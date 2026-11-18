import '@testing-library/jest-dom';
import { jest } from '@jest/globals';
import * as sessionTimeout from '../Scripts/tpr-session-timeout-message';

let dialog;
let button;
let config;
let controller;

beforeEach(() => {
    jest.useFakeTimers();
    jest.setSystemTime(new Date('2026-01-01T12:00:00Z'));
    document.body.innerHTML = `
        <dialog class="tpr-session-timeout-warning" role="alertdialog" aria-describedby="description">
            <div id="description" data-session-timeout-content>Your session is about to expire. {remainingTime}</div>
            <button type="button" data-session-timeout-extend data-session-timeout-extend-url="/session/extend">Continue</button>
        </dialog>`;
    dialog = document.querySelector('dialog');
    button = dialog.querySelector('button');
    dialog.showModal = jest.fn(() => { dialog.open = true; });
    dialog.close = jest.fn(() => { dialog.open = false; });
    config = {
        timeoutMs: 180000,
        extensionMs: 300000,
        warningMs: 60000,
        extendSession: jest.fn().mockResolvedValue(true)
    };
});

afterEach(() => {
    controller?.destroy();
    controller = undefined;
    jest.useRealTimers();
    jest.restoreAllMocks();
    document.body.innerHTML = '';
});

function initialise(options = config) {
    controller = sessionTimeout.initSessionTimeout(dialog, options);
    return controller;
}

test('initialises expiry using the consuming application timeout', () => {
    const now = Date.now();
    initialise();
    expect(controller.expiryTime).toBe(now + config.timeoutMs);
    expect(dialog.open).toBe(false);
    expect(config.extendSession).not.toHaveBeenCalled();
});

test('opens the existing warning exactly at the configured warning point', () => {
    initialise();
    jest.advanceTimersByTime(config.timeoutMs - config.warningMs - 1);
    expect(dialog.showModal).not.toHaveBeenCalled();
    jest.advanceTimersByTime(1);
    expect(dialog.showModal).toHaveBeenCalledTimes(1);
    expect(dialog.open).toBe(true);
    expect(dialog.querySelector('#description')).toHaveTextContent('Your session is about to expire.');
});

test('updates the visible remaining time during the warning', () => {
    config.warningMs = 5000;
    initialise();
    jest.advanceTimersByTime(config.timeoutMs - config.warningMs);
    const countdown = dialog.querySelector('[data-session-timeout-countdown]');
    expect(countdown).toHaveTextContent('5 seconds');
    jest.advanceTimersByTime(1000);
    expect(countdown).toHaveTextContent('4 seconds');
});

test.each([
    [1800000, '30 minutes'],
    [121000, '3 minutes'],
    [120000, '2 minutes'],
    [61000, '2 minutes'],
    [60001, '2 minutes'],
    [60000, '60 seconds'],
    [59000, '59 seconds'],
    [1000, '1 second'],
    [1, '1 second']
])('formats %i milliseconds as %s by default', (remaining, expected) => {
    initialise({ ...config, timeoutMs: remaining + 1000, warningMs: remaining });
    jest.advanceTimersByTime(1000);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe(expected);
});

test('stops at zero and never displays negative time', () => {
    initialise();
    jest.advanceTimersByTime(config.timeoutMs);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('0 seconds');
    jest.advanceTimersByTime(10000);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('0 seconds');
    expect(jest.getTimerCount()).toBe(0);
});

test('calls the application extension mechanism with the configured button URL', () => {
    initialise();
    jest.advanceTimersByTime(config.timeoutMs - config.warningMs);
    button.click();
    expect(config.extendSession).toHaveBeenCalledTimes(1);
    expect(config.extendSession).toHaveBeenCalledWith('/session/extend');
});

test.each([120000, 180000])('resets expiry only after successful extension at %i ms', async (elapsed) => {
    let completeExtension;
    config.extendSession.mockImplementation(() => new Promise(resolve => { completeExtension = resolve; }));
    initialise();
    const originalExpiry = controller.expiryTime;
    jest.advanceTimersByTime(elapsed);
    button.click();
    await jest.advanceTimersByTimeAsync(1000);
    expect(controller.expiryTime).toBe(originalExpiry);
    completeExtension(true);
    await jest.advanceTimersByTimeAsync(0);
    expect(controller.expiryTime).toBe(Date.now() + config.extensionMs);
    expect(dialog.open).toBe(false);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('5 minutes');
    expect(jest.getTimerCount()).toBe(1);
    jest.advanceTimersByTime(config.extensionMs - config.warningMs - 1);
    expect(dialog.open).toBe(false);
    jest.advanceTimersByTime(1);
    expect(dialog.open).toBe(true);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('60 seconds');
});

test.each([
    ['false', () => Promise.resolve(false)],
    ['unconfirmed result', () => Promise.resolve(undefined)],
    ['truthy non-boolean result', () => Promise.resolve('true')],
    ['rejection', () => Promise.reject(new Error('Network unavailable'))],
    ['synchronous exception', () => { throw new Error('Extension failed'); }]
])('preserves expiry and reports extension failure on %s', async (_label, extendSession) => {
    const log = jest.spyOn(console, 'error').mockImplementation(() => {});
    const onError = jest.fn();
    dialog.addEventListener('tpr-session-timeout:error', onError);
    initialise({ ...config, extendSession });
    const originalExpiry = controller.expiryTime;
    jest.advanceTimersByTime(config.timeoutMs - config.warningMs);
    button.click();
    await jest.advanceTimersByTimeAsync(1000);
    expect(controller.expiryTime).toBe(originalExpiry);
    expect(dialog.open).toBe(true);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('59 seconds');
    expect(onError).toHaveBeenCalledTimes(1);
    expect(onError.mock.calls[0][0].detail).toBeInstanceOf(Error);
    expect(log).toHaveBeenCalledTimes(1);
});

test('reinitialisation retains one controller, countdown, timer and click handler', () => {
    const first = initialise();
    const originalExpiry = first.expiryTime;
    jest.advanceTimersByTime(1000);
    initialise();
    expect(controller).toBe(first);
    expect(controller.expiryTime).toBe(originalExpiry);
    expect(jest.getTimerCount()).toBe(1);
    expect(dialog.querySelectorAll('[data-session-timeout-countdown]')).toHaveLength(1);
    button.click();
    expect(config.extendSession).toHaveBeenCalledTimes(1);
});

test('ignores repeated extension clicks while the application request is pending', async () => {
    let completeExtension;
    config.extendSession.mockImplementation(() => new Promise(resolve => { completeExtension = resolve; }));
    initialise();
    button.click();
    button.click();
    button.dispatchEvent(new Event('click'));
    expect(config.extendSession).toHaveBeenCalledTimes(1);
    completeExtension(true);
    await jest.advanceTimersByTimeAsync(0);
    expect(button.disabled).toBe(false);
    button.click();
    expect(config.extendSession).toHaveBeenCalledTimes(2);
    completeExtension(true);
    await jest.advanceTimersByTimeAsync(0);
});

test('calculates remaining time from the clock when timer callbacks are delayed', () => {
    initialise();
    const expiry = controller.expiryTime;
    jest.setSystemTime(expiry - 28500);
    jest.advanceTimersByTime(1000);
    expect(dialog.open).toBe(true);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('28 seconds');
    jest.setSystemTime(expiry + 10000);
    jest.advanceTimersByTime(1000);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('0 seconds');
    expect(jest.getTimerCount()).toBe(0);
});

test.each(['visibilitychange', 'pageshow'])('refreshes immediately on %s after browser suspension', (eventName) => {
    initialise();
    jest.setSystemTime(controller.expiryTime - 25500);
    const target = eventName === 'pageshow' ? window : document;
    target.dispatchEvent(new Event(eventName));
    expect(dialog.open).toBe(true);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('26 seconds');
    jest.setSystemTime(controller.expiryTime + 5000);
    target.dispatchEvent(new Event(eventName));
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('0 seconds');
    expect(jest.getTimerCount()).toBe(0);
});

test('keeps the in-content countdown readable without per-second live announcements', () => {
    initialise();
    jest.advanceTimersByTime(config.timeoutMs - config.warningMs);
    const countdown = dialog.querySelector('[data-session-timeout-countdown]');
    const description = document.getElementById(dialog.getAttribute('aria-describedby'));
    expect(countdown).toHaveAttribute('role', 'timer');
    expect(countdown).toHaveAttribute('aria-live', 'off');
    expect(countdown).not.toHaveAttribute('aria-hidden', 'true');
    expect(description.contains(countdown)).toBe(true);
    expect(description.querySelector('[aria-live="polite"], [aria-live="assertive"], [role="alert"], [role="status"]')).toBeNull();
    jest.advanceTimersByTime(3000);
    expect(countdown.textContent).toBe('57 seconds');
    expect(dialog.showModal).toHaveBeenCalledTimes(1);
    expect(description).toHaveTextContent('Your session is about to expire.');
});

test('honours warning and expiry boundaries that are not whole seconds', () => {
    initialise({ ...config, timeoutMs: 2600, warningMs: 1100 });
    jest.advanceTimersByTime(1499);
    expect(dialog.open).toBe(false);
    jest.advanceTimersByTime(1);
    expect(dialog.open).toBe(true);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('2 seconds');
    jest.advanceTimersByTime(1100);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('0 seconds');
    expect(jest.getTimerCount()).toBe(0);
});

test.each([
    ['timeoutMs', 0],
    ['timeoutMs', -1],
    ['timeoutMs', undefined],
    ['timeoutMs', '180000'],
    ['timeoutMs', Infinity],
    ['extensionMs', 0],
    ['extensionMs', NaN],
    ['warningMs', -1],
    ['warningMs', Infinity],
    ['warningMs', 180001]
])('rejects invalid %s value %s before starting timers or changing markup', (name, value) => {
    expect(() => initialise({ ...config, [name]: value })).toThrow(RangeError);
    expect(jest.getTimerCount()).toBe(0);
    expect(dialog.querySelector('[data-session-timeout-countdown]')).toBeNull();
});

test('rejects a missing extension mechanism', () => {
    expect(() => initialise({ ...config, extendSession: undefined })).toThrow(TypeError);
    expect(jest.getTimerCount()).toBe(0);
});

test('destroy prevents pending extension completion from reviving a timer', async () => {
    let completeExtension;
    config.extendSession.mockImplementation(() => new Promise(resolve => { completeExtension = resolve; }));
    initialise();
    const originalExpiry = controller.expiryTime;
    button.click();
    controller.destroy();
    completeExtension(true);
    await jest.advanceTimersByTimeAsync(0);
    expect(controller.expiryTime).toBe(originalExpiry);
    expect(jest.getTimerCount()).toBe(0);
    expect(dialog.querySelector('[data-session-timeout-countdown]')).toBeNull();
    document.dispatchEvent(new Event('visibilitychange'));
    window.dispatchEvent(new Event('pageshow'));
    button.click();
    expect(config.extendSession).toHaveBeenCalledTimes(1);
    expect(jest.getTimerCount()).toBe(0);
});

test('destroy is idempotent and allows clean reinitialisation', () => {
    const first = initialise();
    first.destroy();
    const second = initialise();
    first.destroy();
    expect(initialise()).toBe(second);
    expect(second).not.toBe(first);
    expect(jest.getTimerCount()).toBe(1);
    expect(dialog.querySelectorAll('[data-session-timeout-countdown]')).toHaveLength(1);
});

test.each([
    '<p>Your session expires in {remainingTime}.</p>',
    '<p>Your session expires in <strong>{remainingTime}</strong>.</p>'
])('replaces a content placeholder inline without Umbraco: %s', (html) => {
    const content = dialog.querySelector('[data-session-timeout-content]');
    content.innerHTML = html;
    initialise();
    jest.advanceTimersByTime(config.timeoutMs - config.warningMs);
    const countdown = content.querySelector('[data-session-timeout-countdown]');
    expect(content.textContent).toBe('Your session expires in 60 seconds.');
    expect(countdown.tagName).toBe('SPAN');
    expect(dialog.querySelectorAll('[data-session-timeout-countdown]')).toHaveLength(1);
    expect(content.innerHTML).toContain(html.includes('<strong>') ? '<strong>' : '<p>');
    jest.advanceTimersByTime(1000);
    expect(content.textContent).toBe('Your session expires in 59 seconds.');
    controller.destroy();
    expect(content.innerHTML).toBe(html);
    initialise();
    expect(content.textContent).toBe('Your session expires in 3 minutes.');
});

test.each([true, false])('does not insert a countdown without a token (content hook: %s)', async (hasContentHook) => {
    const content = dialog.querySelector('[data-session-timeout-content]');
    content.innerHTML = '<p>Your session is about to expire.</p>';
    if (!hasContentHook) content.removeAttribute('data-session-timeout-content');
    const originalMarkup = dialog.innerHTML;
    initialise();
    expect(dialog.querySelector('[data-session-timeout-countdown]')).toBeNull();
    expect(dialog.innerHTML).toBe(originalMarkup);
    jest.advanceTimersByTime(config.timeoutMs - config.warningMs);
    expect(dialog.open).toBe(true);
    expect(dialog.innerHTML).toBe(originalMarkup);
    button.click();
    await jest.advanceTimersByTimeAsync(0);
    expect(config.extendSession).toHaveBeenCalledTimes(1);
    expect(controller.expiryTime).toBe(Date.now() + config.extensionMs);
    expect(dialog.open).toBe(false);
    expect(dialog.innerHTML).toBe(originalMarkup);
    jest.advanceTimersByTime(config.extensionMs);
    expect(dialog.open).toBe(true);
    expect(jest.getTimerCount()).toBe(0);
    expect(dialog.innerHTML).toBe(originalMarkup);
    controller.destroy();
    expect(dialog.innerHTML).toBe(originalMarkup);
});

test('updates every placeholder without replacing surrounding rich-text elements', () => {
    const content = dialog.querySelector('[data-session-timeout-content]');
    content.innerHTML = '<p>{remainingTime} remaining. <strong>{remainingTime}</strong> <a href="/help">Help</a></p>';
    const link = content.querySelector('a');
    const onClick = jest.fn();
    link.addEventListener('click', onClick);
    initialise();
    jest.advanceTimersByTime(config.timeoutMs - config.warningMs);
    expect(content.querySelectorAll('[data-session-timeout-countdown]')).toHaveLength(2);
    expect(content.textContent).toBe('60 seconds remaining. 60 seconds Help');
    expect(content.querySelector('a')).toBe(link);
    link.dispatchEvent(new Event('click'));
    expect(onClick).toHaveBeenCalledTimes(1);
});

test('uses an application formatter for initial, delayed, extended and expired remaining time', async () => {
    const formatRemainingTime = jest.fn(remaining => `Remaining: ${remaining}`);
    initialise({ ...config, formatRemainingTime });
    const countdown = dialog.querySelector('[data-session-timeout-countdown]');
    expect(countdown.textContent).toBe('Remaining: 180000');
    jest.setSystemTime(controller.expiryTime - 25500);
    document.dispatchEvent(new Event('visibilitychange'));
    expect(countdown.textContent).toBe('Remaining: 25500');
    button.click();
    await jest.advanceTimersByTimeAsync(0);
    expect(countdown.textContent).toBe('Remaining: 300000');
    jest.setSystemTime(controller.expiryTime + 1000);
    document.dispatchEvent(new Event('visibilitychange'));
    expect(countdown.textContent).toBe('Remaining: 0');
    expect(formatRemainingTime).toHaveBeenLastCalledWith(0);
});

test('documented formatter displays seconds throughout the countdown', () => {
    initialise({
        ...config,
        warningMs: 120000,
        formatRemainingTime: remainingMs => Math.ceil(remainingMs / 1000) + 's'
    });
    const countdown = dialog.querySelector('[data-session-timeout-countdown]');
    expect(countdown.textContent).toBe('180s');
    jest.advanceTimersByTime(60000);
    expect(dialog.open).toBe(true);
    expect(countdown.textContent).toBe('120s');
    jest.advanceTimersByTime(1000);
    expect(countdown.textContent).toBe('119s');
    jest.advanceTimersByTime(118000);
    expect(countdown.textContent).toBe('1s');
    jest.advanceTimersByTime(1000);
    expect(countdown.textContent).toBe('0s');
    jest.advanceTimersByTime(1000);
    expect(countdown.textContent).toBe('0s');
});

test('rejects a configured formatter that is not a function', () => {
    expect(() => initialise({ ...config, formatRemainingTime: 'minutes' })).toThrow(TypeError);
    expect(jest.getTimerCount()).toBe(0);
});

test('default formatting holds minutes until the final 60-second countdown', () => {
    initialise({ ...config, timeoutMs: 120000, warningMs: 120000 });
    const countdown = dialog.querySelector('[data-session-timeout-countdown]');
    expect(countdown.textContent).toBe('2 minutes');
    const staticText = countdown.firstChild;
    jest.advanceTimersByTime(59000);
    expect(countdown.textContent).toBe('2 minutes');
    expect(countdown.firstChild).toBe(staticText);
    jest.advanceTimersByTime(1000);
    expect(countdown.textContent).toBe('60 seconds');
    jest.advanceTimersByTime(1000);
    expect(countdown.textContent).toBe('59 seconds');
    jest.advanceTimersByTime(59000);
    expect(countdown.textContent).toBe('0 seconds');
});

test('emits one bubbling expiry event at zero, even without a countdown token', () => {
    dialog.querySelector('[data-session-timeout-content]').textContent = 'Your session is about to expire.';
    const onExpired = jest.fn();
    const parent = dialog.parentElement;
    parent.addEventListener('tpr-session-timeout:expired', onExpired);
    try {
        initialise();
        jest.advanceTimersByTime(config.timeoutMs - 1);
        expect(onExpired).not.toHaveBeenCalled();
        jest.advanceTimersByTime(1);
        expect(onExpired).toHaveBeenCalledTimes(1);
        expect(onExpired.mock.calls[0][0].target).toBe(dialog);
        document.dispatchEvent(new Event('visibilitychange'));
        window.dispatchEvent(new Event('pageshow'));
        jest.advanceTimersByTime(5000);
        expect(onExpired).toHaveBeenCalledTimes(1);
    } finally {
        parent.removeEventListener('tpr-session-timeout:expired', onExpired);
    }
});

test('emits expiry for the extended deadline, not the original deadline', async () => {
    const onExpired = jest.fn();
    dialog.addEventListener('tpr-session-timeout:expired', onExpired);
    initialise();
    jest.advanceTimersByTime(config.timeoutMs - config.warningMs);
    button.click();
    await jest.advanceTimersByTimeAsync(0);
    jest.advanceTimersByTime(config.warningMs);
    expect(onExpired).not.toHaveBeenCalled();
    jest.advanceTimersByTime(config.extensionMs - config.warningMs);
    expect(onExpired).toHaveBeenCalledTimes(1);
    button.click();
    await jest.advanceTimersByTimeAsync(0);
    jest.setSystemTime(controller.expiryTime + 1000);
    document.dispatchEvent(new Event('visibilitychange'));
    expect(onExpired).toHaveBeenCalledTimes(2);
    controller.destroy();
    window.dispatchEvent(new Event('pageshow'));
    expect(onExpired).toHaveBeenCalledTimes(2);
});

test('reads shared dialog timing attributes when only the application extension callback is supplied', async () => {
    dialog.dataset.timeoutMs = '125000';
    dialog.dataset.extensionMs = '180000';
    dialog.dataset.warningMs = '120000';
    initialise({ extendSession: config.extendSession });
    expect(controller.expiryTime).toBe(Date.now() + 125000);
    jest.advanceTimersByTime(4999);
    expect(dialog.open).toBe(false);
    jest.advanceTimersByTime(1);
    expect(dialog.open).toBe(true);
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('2 minutes');
    button.click();
    await jest.advanceTimersByTimeAsync(0);
    expect(controller.expiryTime).toBe(Date.now() + 180000);
    expect(dialog.open).toBe(false);
});

test('explicit JavaScript durations override dialog attributes', () => {
    dialog.dataset.timeoutMs = '125000';
    dialog.dataset.extensionMs = '180000';
    dialog.dataset.warningMs = '120000';
    initialise();
    expect(controller.expiryTime).toBe(Date.now() + config.timeoutMs);
    jest.advanceTimersByTime(5000);
    expect(dialog.open).toBe(false);
});

test.each(['', 'invalid', '-1'])('rejects invalid dialog timing %s before changing content', value => {
    dialog.dataset.timeoutMs = '125000';
    dialog.dataset.extensionMs = '180000';
    dialog.dataset.warningMs = value;
    expect(() => initialise({ extendSession: config.extendSession })).toThrow(RangeError);
    expect(dialog.querySelector('[data-session-timeout-countdown]')).toBeNull();
});

test('navigates to the configured expiry URL once the countdown reaches zero', () => {
    dialog.dataset.sessionTimeoutExpiryUrl = '#session-expired';
    const originalUrl = window.location.href;
    try {
        initialise();
        jest.advanceTimersByTime(config.timeoutMs - 1);
        expect(window.location.hash).toBe('');
        jest.advanceTimersByTime(1);
        expect(window.location.hash).toBe('#session-expired');
    } finally {
        window.history.replaceState(null, '', originalUrl);
    }
});

test('allows an application to cancel default expiry navigation and handle its own request', () => {
    dialog.dataset.sessionTimeoutExpiryUrl = '#session-expired';
    dialog.addEventListener('tpr-session-timeout:expired', event => event.preventDefault());
    const originalUrl = window.location.href;
    try {
        initialise();
        jest.advanceTimersByTime(config.timeoutMs);
        expect(window.location.hash).toBe('');
    } finally {
        window.history.replaceState(null, '', originalUrl);
    }
});

test.each(['javascript:alert(1)', 'data:text/html,test', 'https://[invalid'])('rejects unsafe or invalid expiry URL %s', expiryUrl => {
    expect(() => initialise({ ...config, expiryUrl })).toThrow(/expiryUrl|Invalid URL/);
    expect(jest.getTimerCount()).toBe(0);
    expect(dialog.querySelector('[data-session-timeout-countdown]')).toBeNull();
});
