export class ErrorInfoPageObject {
    private element: HTMLElement;

    constructor(element: HTMLElement) {
        this.element = element;
    }

    getMainMessageText(): string {
        const message: HTMLElement = this.element.querySelector('.main-message');
        return message.textContent;
    }

    getDescriptionMessageText(): string {
        const message: HTMLElement = this.element.querySelector('.error-description');
        return message.textContent;
    }

    clickDismiss(): void {
        const button: HTMLButtonElement = this.element.querySelector('.header-dismiss');
        button.click();
    }

    clickExpansionPanelHeader(): void {
        const panel: HTMLButtonElement = this.element.querySelector('.expansion-icon');
        panel.click();
    }
}