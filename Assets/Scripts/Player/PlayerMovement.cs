using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] AudioSource _stickySound;

    private Player _player;
    private Vector3 _moveVector;

    private void OnEnable()
    {
        _player = GetComponent<Player>();
    }

    private void Move()
    {
        //_moveVector = Vector3.zero;
        _moveVector.x = _player._joystick.Horizontal * _player.characterMoveSpeed * Time.deltaTime;
        _moveVector.z = _player._joystick.Vertical * _player.characterMoveSpeed * Time.deltaTime;

        if (_player._joystick.Horizontal != 0 || _player._joystick.Vertical != 0)
        {
            Vector3 direction = Vector3.RotateTowards(transform.forward, _moveVector, _player.characterRotateSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);

            if (_player._boxManager.GetHaveBox())
            {
                _player.characterAnimatorController.BoxRun();
            }
            else
            {
                if (_player.StandingOnStickyLiquid)
                {
                    if (!_stickySound.isPlaying)
                        _stickySound.Play();

                    _player.characterAnimatorController.SlimeWalk();
                }
                else
                {
                    _player.characterAnimatorController.PlayRun();
                }
            }
        }
        else if (_player._joystick.Horizontal == 0 && _player._joystick.Vertical == 0)
        {
            if (_player._boxManager.GetHaveBox())
            {
                _player.characterAnimatorController.BoxStand();
            }
            else
            {
                _player.characterAnimatorController.PlayIdle();
            }
        }
        _player.characterRigidbody.MovePosition(_player.characterRigidbody.position + _moveVector);
    }

    private void FixedUpdate()
    {
        if (_player.CanMove)
        {
            Move();
        }
    }
}