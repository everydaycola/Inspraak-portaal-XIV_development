import { Modal } from 'bootstrap';

export function configureAutoOpeningModals(){
    document.addEventListener('DOMContentLoaded', function () {
        var modalRoot = document.getElementById('modal-root');
        if (!modalRoot) return;

        var openModalId = modalRoot.getAttribute('data-open-modal');
        if (openModalId) {
            var modalElement = document.getElementById(openModalId);
            if (modalElement) {
                var modal = new Modal(modalElement);
                modal.show();
            }
        }
    });
}