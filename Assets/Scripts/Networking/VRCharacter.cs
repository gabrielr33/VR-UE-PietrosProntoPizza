using Input;
using Photon.Pun;
using UnityEngine;

namespace Networking
{
    public class VRCharacter : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _body;
        [SerializeField] private Transform _left;
        [SerializeField] private Transform _right;

        private Transform _headRoot;
        private Transform _rightControllerRoot;
        private Transform _leftControllerRoot;

        private Vector3 _headPosition;
        private Quaternion _headRotation;

        private bool _trackingLocally;

        private GameManager _gameManager;
        private PlayerInputController _playerInputs;
        private bool _playerIsReadyToRace;

        private bool _resetPositions;

        private void Awake()
        {
            if (transform.GetComponent<PhotonView>().IsMine)
            {
                _trackingLocally = true;

                _headRoot = GameObject.FindWithTag("MainCamera").transform;
                _rightControllerRoot = GameObject.FindWithTag("RightController").transform;
                _leftControllerRoot = GameObject.FindWithTag("LeftController").transform;

                _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
                _playerInputs = GetComponent<PlayerInputController>();

                _body.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        private void Update()
        {
            if (_trackingLocally)
            {
                _headPosition = _headRoot.position;
                _headRotation = _headRoot.rotation;

                transform.position = new Vector3(_headPosition.x, transform.position.y, _headPosition.z);
                transform.rotation = Quaternion.Euler(0f, _headRotation.eulerAngles.y, 0f);

                _head.position = _headPosition;
                _head.rotation = _headRotation;

                _body.position = new Vector3(_headPosition.x, _headPosition.y - 0.3f, _headPosition.z);
                

                _left.position = _leftControllerRoot.position;
                _left.rotation = _leftControllerRoot.rotation;

                _right.position = _rightControllerRoot.position;
                _right.rotation = _rightControllerRoot.rotation;

                HandleControllerInputs();
            }
        }

        private void HandleControllerInputs()
        {
            if (_playerInputs.InputButtons.ButtonPrimary_Right > 0.9f)
            {
                // TODO
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);

                stream.SendNext(_head.position);
                stream.SendNext(_head.rotation);

                stream.SendNext(_body.position);
                
                stream.SendNext(_left.position);
                stream.SendNext(_left.rotation);

                stream.SendNext(_right.position);
                stream.SendNext(_right.rotation);
            }
            else
            {
                // Network player, receive data
                transform.position = (Vector3)stream.ReceiveNext();
                transform.rotation = (Quaternion)stream.ReceiveNext();

                _head.position = (Vector3)stream.ReceiveNext();
                _head.rotation = (Quaternion)stream.ReceiveNext();

                _body.position = (Vector3)stream.ReceiveNext();

                _left.position = (Vector3)stream.ReceiveNext();
                _left.rotation = (Quaternion)stream.ReceiveNext();

                _right.position = (Vector3)stream.ReceiveNext();
                _right.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}