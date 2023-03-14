using UnityEngine;
using UnityEngine.InputSystem;
using Ipt = UnityEngine.Input;


namespace Input
{
    /// <summary>
    /// This is all based off an xbox controller, but the inputs can be remapped to keyboard.
    /// </summary>
    public struct InputContext
    {
        /// <summary>
        /// Positive x is left, positive y is up. Input is normalized.
        /// </summary>
        public Vector2 LeftStick;

        /// <summary>
        /// Positive x is left, positive y is up. Input is normalized.
        /// </summary>
        public Vector2 RightStick;
    
        /// <summary>
        /// Positive x is left, positive y is up. Input is normalized.
        /// </summary>
        public Vector2 DPad;

        public bool GetKeyA;
    
        public bool GetKeyDownA;

        public bool GetKeyUpA;
    
        public bool GetKeyB;
    
        public bool GetKeyDownB;

        public bool GetKeyUpB;
    
        public bool GetKeyX;
    
        public bool GetKeyDownX;

        public bool GetKeyUpX;
    
        public bool GetKeyY;
    
        public bool GetKeyDownY;

        public bool GetKeyUpY;
    
        public bool GetKeyLB;
    
        public bool GetKeyDownLB;

        public bool GetKeyUpLB;
    
        public bool GetKeyRB;
    
        public bool GetKeyDownRB;

        public bool GetKeyUpRB;
    
        public bool GetKeyLT;
    
        public bool GetKeyDownLT;

        public bool GetKeyUpLT;
    
        public bool GetKeyRT;
    
        public bool GetKeyDownRT;

        public bool GetKeyUpRT;
        
        public bool GetKeyLeftStickPress;
    
        public bool GetKeyDownLeftStickPress;

        public bool GetKeyUpLeftStickPress;
    
        public bool GetKeyRightStickPress;
    
        public bool GetKeyDownRightStickPress;

        public bool GetKeyUpRightStickPress;
    }
    
    /// <summary>
    /// If LateUpdate, OnEnable, or OnDisable have to be used, make sure you override and call base.TheOverridenMethod().
    /// </summary>
    public abstract class InputMonoBehaviour : MonoBehaviour
    {
        protected InputContext CurrentInput { get; private set; }
        private MainInput _input;
        private InputAction _leftStick;
        private InputAction _rightStick;
        private InputAction _dPad;
        private InputAction _a;
        private InputAction _b;
        private InputAction _x;
        private InputAction _y;
        private InputAction _lb;
        private InputAction _rb;
        private InputAction _lt;
        private InputAction _rt;
        private InputAction _leftStickPress;
        private InputAction _rightStickPress;

        protected virtual void OnEnable()
        {
            _input = new MainInput();
            _leftStick = _input.Player.LeftStick;
            _rightStick = _input.Player.RightStick;
            _dPad = _input.Player.DPad;
            _a = _input.Player.A;
            _b = _input.Player.B;
            _x = _input.Player.X;
            _y = _input.Player.Y;
            _lb = _input.Player.LB;
            _rb = _input.Player.RB;
            _lt = _input.Player.LT;
            _rt = _input.Player.RT;
            _leftStickPress = _input.Player.LeftStickPress;
            _rightStickPress = _input.Player.RightStickPress;
            _leftStick.Enable();
            _rightStick.Enable();
            _dPad.Enable();
            _a.Enable();
            _b.Enable();
            _x.Enable();
            _y.Enable();
            _lb.Enable();
            _rb.Enable();
            _lt.Enable();
            _rt.Enable();
            _leftStickPress.Enable();
            _rightStickPress.Enable();
        }
        protected virtual void LateUpdate()
        {
            CurrentInput = new InputContext
            {
                //collect inputs
                LeftStick = _leftStick.ReadValue<Vector2>(),
                RightStick = _rightStick.ReadValue<Vector2>(),
                DPad = _dPad.ReadValue<Vector2>(),
                GetKeyA = _a.IsPressed(),
                GetKeyB = _b.IsPressed(),
                GetKeyX = _x.IsPressed(),
                GetKeyY = _y.IsPressed(),
                GetKeyLB = _lb.IsPressed(),
                GetKeyRB = _rb.IsPressed(),
                GetKeyLT = _lt.IsPressed(),
                GetKeyRT = _rt.IsPressed(),
                GetKeyLeftStickPress = _leftStickPress.IsPressed(),
                GetKeyRightStickPress = _rightStickPress.IsPressed(),
                GetKeyDownA = _a.WasPressedThisFrame(),
                GetKeyDownB = _b.WasPressedThisFrame(),
                GetKeyDownX = _x.WasPressedThisFrame(),
                GetKeyDownY = _y.WasPressedThisFrame(),
                GetKeyDownLB = _lb.WasPressedThisFrame(),
                GetKeyDownRB = _rb.WasPressedThisFrame(),
                GetKeyDownLT = _lt.WasPressedThisFrame(),
                GetKeyDownRT = _rt.WasPressedThisFrame(),
                GetKeyDownLeftStickPress = _leftStickPress.WasPressedThisFrame(),
                GetKeyDownRightStickPress = _rightStickPress.WasPressedThisFrame(),
                GetKeyUpA = _a.WasReleasedThisFrame(),
                GetKeyUpB = _b.WasReleasedThisFrame(),
                GetKeyUpX = _x.WasReleasedThisFrame(),
                GetKeyUpY = _y.WasReleasedThisFrame(),
                GetKeyUpLB = _lb.WasReleasedThisFrame(),
                GetKeyUpRB = _rb.WasReleasedThisFrame(),
                GetKeyUpLT = _lt.WasReleasedThisFrame(),
                GetKeyUpRT = _rt.WasReleasedThisFrame(),
                GetKeyUpLeftStickPress = _leftStickPress.WasReleasedThisFrame(),
                GetKeyUpRightStickPress = _rightStickPress.WasReleasedThisFrame()
            };
        }

        protected virtual void OnDisable()
        {
            _leftStick.Disable();
            _rightStick.Disable();
            _dPad.Disable();
            _a.Disable();
            _b.Disable();
            _x.Disable();
            _y.Disable();
            _lb.Disable();
            _rb.Disable();
            _lt.Disable();
            _rt.Disable();
            _leftStickPress.Disable();
            _rightStickPress.Disable();
        }
    }
}