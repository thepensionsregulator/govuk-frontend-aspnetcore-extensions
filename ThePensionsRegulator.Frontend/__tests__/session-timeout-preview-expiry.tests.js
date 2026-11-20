import '@testing-library/jest-dom';
import { jest } from '@jest/globals';
import { initSessionTimeout } from '../Scripts/tpr-session-timeout-message';

jest.unstable_mockModule('/ThePensionsRegulator.Frontend/js/tpr-session-timeout-message.js', () => ({ initSessionTimeout }), { virtual: true });

let dialog;
let controller;
let returnToExamples;

beforeEach(() => {
    jest.resetModules();
    jest.useFakeTimers();
    document.body.innerHTML = `
        <div>
            <dialog class="tpr-session-timeout-warning" data-module="tpr-session-timeout"
                data-timeout-ms="125000" data-extension-ms="180000" data-warning-ms="120000"
                data-session-timeout-expiry-url="#session-timeout">
                <div data-session-timeout-content>Expires in {remainingTime}.</div>
                <button data-session-timeout-extend>Continue</button>
            </dialog>
        </div>`;
    dialog = document.querySelector('dialog');
    dialog.showModal = jest.fn(() => { dialog.open = true; });
    dialog.close = jest.fn(() => { dialog.open = false; });
    returnToExamples = jest.fn();
    dialog.addEventListener('tpr-session-timeout:expired', returnToExamples);
});

afterEach(() => {
    controller?.destroy();
    controller = undefined;
    window.history.replaceState(null, '', window.location.pathname);
    jest.useRealTimers();
    document.body.innerHTML = '';
});

async function loadPreview() {
    await import('../../GovUk.Frontend.Umbraco.ExampleApp/wwwroot/js/session-timeout-preview.js');
    controller = initSessionTimeout(dialog);
}

test('returns to the examples page only when the countdown reaches zero', async () => {
    await loadPreview();
    jest.advanceTimersByTime(124999);
    expect(returnToExamples).not.toHaveBeenCalled();
    expect(window.location.hash).toBe('');
    jest.advanceTimersByTime(1);
    expect(returnToExamples).toHaveBeenCalledTimes(1);
    expect(window.location.hash).toBe('#session-timeout');
    expect(dialog.querySelector('[data-session-timeout-countdown]').textContent).toBe('0 seconds');
});

test('successful extension postpones the return until the new expiry', async () => {
    await loadPreview();
    jest.advanceTimersByTime(5000);
    dialog.querySelector('button').click();
    await jest.advanceTimersByTimeAsync(0);
    jest.advanceTimersByTime(120000);
    expect(returnToExamples).not.toHaveBeenCalled();
    expect(window.location.hash).toBe('');
    jest.advanceTimersByTime(60000);
    expect(returnToExamples).toHaveBeenCalledTimes(1);
    expect(window.location.hash).toBe('#session-timeout');
});

test('returns once when a suspended preview resumes after expiry', async () => {
    await loadPreview();
    jest.setSystemTime(controller.expiryTime + 5000);
    document.dispatchEvent(new Event('visibilitychange'));
    window.dispatchEvent(new Event('pageshow'));
    expect(returnToExamples).toHaveBeenCalledTimes(1);
    expect(window.location.hash).toBe('#session-timeout');
});

test('does not initialise static variants without runtime settings', async () => {
    dialog.removeAttribute('data-module');
    await import('../../GovUk.Frontend.Umbraco.ExampleApp/wwwroot/js/session-timeout-preview.js');
    expect(jest.getTimerCount()).toBe(0);
    expect(dialog.showModal).not.toHaveBeenCalled();
    expect(dialog.querySelector('[data-session-timeout-countdown]')).toBeNull();
});
