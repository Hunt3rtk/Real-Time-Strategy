using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour {
        //GameManager
        public GameManager gm;

        //Control Input
        public PlayerInput playerInput;

        private Vector3 lastPosition;

        [SerializeField]
        private LayerMask placementLayermask;

        public void Enable() {
            playerInput = new PlayerInput();
        }

        public void EnableGameplay() {
            playerInput.Gameplay.Enable();
            playerInput.Gameplay.MultiSelect.performed += MultiSelect;
            playerInput.Gameplay.Select.performed += Select;
            playerInput.Gameplay.Move.performed += Move;
        }

        public void EnableBuilding() {
            playerInput.Building.Enable();
            playerInput.Building.Place.performed += Place;
            playerInput.Building.Cancel.performed += Cancel;

        }

        public void DisableGameplay() {
            playerInput.Gameplay.Disable();
            playerInput.Gameplay.MultiSelect.performed -= MultiSelect;
            playerInput.Gameplay.Select.performed -= Select;
            playerInput.Gameplay.Move.performed -= Move;
        }

        public void DisableBuilding() {
            playerInput.Building.Disable();
            playerInput.Building.Place.performed -= Place;
            playerInput.Building.Cancel.performed -= Cancel;
        }

        public void EnablePlace() {
            playerInput.Building.Place.performed += Place;
        }

        public void DisablePlace() {
            playerInput.Building.Place.performed -= Place;
        }

        public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

        public Vector3 GetSelectedMapPosition() {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, placementLayermask)) {
                lastPosition = hit.point;
            }
            return lastPosition;
        }

        public void Select(InputAction.CallbackContext ctx) {
            if (IsPointerOverUI()) return;
            if(Keyboard.current.shiftKey.isPressed) return;
            gm.ActivateSelect();
        }

        public void MultiSelect(InputAction.CallbackContext ctx) {
            //if (IsPointerOverUI()) return;
            gm.ActivateMultipleSelect();
        }

        public void Move(InputAction.CallbackContext ctx) {
           gm.ActivateMove();
        }

        public void Place(InputAction.CallbackContext ctx) {
            if (IsPointerOverUI()) return;
            Vector3 mousePosition = GetSelectedMapPosition();
            Debug.Log(mousePosition);
            gm.ActivatePlaceBuilding(mousePosition);
        }

        public void Cancel(InputAction.CallbackContext ctx) {
            gm.ActivateBuildingCancel();
        }
    }
