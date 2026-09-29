import { jest } from "@jest/globals";
import { submitOnEnter } from "../../Scripts/address-lookup/submit-on-enter.js";

describe("submitOnEnter", () => {
	function dispatchKeydown(target: HTMLElement, key: string, isComposing = false): KeyboardEvent {
		const event = new KeyboardEvent("keydown", {
			key,
			isComposing,
			bubbles: true,
			cancelable: true
		});
		target.dispatchEvent(event);
		return event;
	}

	it.each(["input", "select"])("calls the action and prevents default for Enter on a %s", tagName => {
		const container = document.createElement("div");
		const target = document.createElement(tagName);
		const action = jest.fn<() => void>();
		container.appendChild(target);
		submitOnEnter(container, action);

		const event = dispatchKeydown(target, "Enter");

		expect(action).toHaveBeenCalledTimes(1);
		expect(event.defaultPrevented).toBe(true);
	});

	it("does not call the action or prevent default while an IME composition is active", () => {
		const container = document.createElement("div");
		const input = document.createElement("input");
		const action = jest.fn<() => void>();
		container.appendChild(input);
		submitOnEnter(container, action);

		const event = dispatchKeydown(input, "Enter", true);

		expect(action).not.toHaveBeenCalled();
		expect(event.defaultPrevented).toBe(false);
	});

	it("does not call the action or prevent default for other keys", () => {
		const container = document.createElement("div");
		const input = document.createElement("input");
		const action = jest.fn<() => void>();
		container.appendChild(input);
		submitOnEnter(container, action);

		const event = dispatchKeydown(input, "Escape");

		expect(action).not.toHaveBeenCalled();
		expect(event.defaultPrevented).toBe(false);
	});

	it.each(["button", "a", "div"])("ignores Enter from a %s", tagName => {
		const container = document.createElement("div");
		const target = document.createElement(tagName);
		const action = jest.fn<() => void>();
		container.appendChild(target);
		submitOnEnter(container, action);

		const event = dispatchKeydown(target, "Enter");

		expect(action).not.toHaveBeenCalled();
		expect(event.defaultPrevented).toBe(false);
	});
});
