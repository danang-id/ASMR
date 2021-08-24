
import Button from "@asmr/components/Button"
import Modal from "@asmr/components/Modal"
import config from "@asmr/libs/common/config"
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"

interface RegistrationModalProps {
    onClose: () => void
    show: boolean
}
function RegistrationModal({ onClose, show }: RegistrationModalProps): JSX.Element {
    return (
        <Modal onClose={onClose} show={show} title="How can I register?">
            <Modal.Body>
                This is a closed-integrated information system that tied to a specific workplace. 
                To be able to access <strong>{config.application.name}</strong> on your workplace, 
                you need to contact your workplace Administrator and provide the information about 
                your employment information. The Administrator then will be able to register and 
                grant you access to the system.
            </Modal.Body>

            <Modal.Footer>
                <div className="modal-actions">
                    <Button size="sm" onClick={onClose} style="none">I got it!</Button>
                </div>
            </Modal.Footer>
        </Modal>
    )
}

export default RegistrationModal
