export function submitOnEnter(container: HTMLElement, action: () => void): void {
    container.addEventListener("keydown", event => {
        if (event.key !== "Enter" || event.isComposing) {
            return;
        }

        const target = event.target;
        if (!(target instanceof HTMLInputElement || target instanceof HTMLSelectElement)) {
            return;
        }

        event.preventDefault();
        action();
    });
}