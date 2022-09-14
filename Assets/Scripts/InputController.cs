using System;
using UnityEngine;

 
    public class InputController : MonoBehaviour
    {
        public event Action<Vector3> Pressed;
        public event Action<Vector3, Vector3> Moved;
        public event Action<Vector3> Released;
        
        public static Vector2 Joystick { get; private set; }

        [SerializeField] private float _joystickSize = 300;
        [Range(0, 0.8f)][SerializeField] private float _joystickTreshold = 0f;



        private Vector3 _lastInput;

        private void Awake()
        {
            Joystick = Vector2.zero;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Joystick = Vector2.zero;
                _lastInput = Input.mousePosition;
                Pressed?.Invoke(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Joystick = Vector2.zero;
                Released?.Invoke(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                var _inputDelta = Input.mousePosition - _lastInput;
                if (_inputDelta.magnitude < _joystickTreshold * _joystickSize)
                {
                    Joystick = Vector2.zero;
                    return;
                }

                var overshoot = Mathf.Max(_inputDelta.magnitude - _joystickSize, 0f);
                _lastInput += _inputDelta.normalized * overshoot;

                Joystick = (Input.mousePosition - _lastInput) / _joystickSize;

                Moved?.Invoke(Input.mousePosition, Joystick);
            }
        }
    public Vector2 GetJoystick()
    {
        return Joystick;
    }
    }

