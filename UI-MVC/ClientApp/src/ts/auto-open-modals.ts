import { Modal } from 'bootstrap';

/*
* This script can be integrated with 'ModelState.AddModelError("", "Ongeldig tijdstip.");' from .NET
* to automaticcaly open a modal in which something went wrong.
* 
* HOW TO USE ? 
* 
* In your html add following element : "<div id="modal-root" class="hidden" data-open-modal="@ViewBag.OpenModal"></div>"
* In the validation controller side add: "ViewBag.OpenModal = "addWerksessieModal";" on which 'addWerksessieModal' is the ID of the modal we need to reopen.
* Add this script to that html page. 
* Finished! the modal should now open up on modalstate errors.
* */

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
