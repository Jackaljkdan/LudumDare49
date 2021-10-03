using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class BrokenRespawner : MonoBehaviour
    {
        #region Inspector

        public float seconds = 2;

        [SerializeField]
        private Foot footPrefab = null;

        [SerializeField]
        private Transform upperBody = null;

        [SerializeField]
        private Rigidbody torso = null;

        #endregion

        [Inject]
        private Balance balance = null;

        private List<Vector3> localPositions;
        private List<Quaternion> localRotations;
        private List<RigidbodyConstraints> constraints;
        private Vector3 leftFootLocalPosition;
        private Vector3 rightFootLocalPosition;

        private void Start()
        {
            balance.onBalanceLost.AddListener(OnBalanceLost);
            localPositions = new List<Vector3>();
            localRotations = new List<Quaternion>();
            constraints = new List<RigidbodyConstraints>();

            foreach (Transform child in upperBody)
            {
                localPositions.Add(child.localPosition);
                localRotations.Add(child.localRotation);
                if (child.TryGetComponent(out Rigidbody rb))
                    constraints.Add(rb.constraints);
                else
                    constraints.Add(RigidbodyConstraints.FreezeAll);
            }

            var feet = GetComponentsInChildren<Foot>();
            leftFootLocalPosition = feet[0].transform.localPosition;
            rightFootLocalPosition = feet[1].transform.localPosition;
        }

        private void OnBalanceLost()
        {
            StartCoroutine(RespawnCoroutine());
        }

        private IEnumerator RespawnCoroutine()
        {
            yield return new WaitForSeconds(seconds);

            for (int i = 0; i < localPositions.Count; i++)
            {
                Transform child = upperBody.GetChild(i);

                if (child.TryGetComponent(out Rigidbody rb))
                    rb.constraints = constraints[i];

                child.localPosition = localPositions[i];
                child.localRotation = localRotations[i];
            }

            foreach (var foot in GetComponentsInChildren<Foot>())
                Destroy(foot.gameObject);

            var leftFoot = Instantiate(footPrefab, transform);
            leftFoot.name = "FootL";
            leftFoot.transform.localPosition = leftFootLocalPosition;
            leftFoot.GetComponent<CharacterJoint>().connectedBody = torso;
            leftFoot.balance = balance;

            var rightFoot = Instantiate(footPrefab, transform);
            rightFoot.name = "FootR";
            rightFoot.transform.localPosition = rightFootLocalPosition;
            rightFoot.GetComponent<CharacterJoint>().connectedBody = torso;
            rightFoot.balance = balance;

            //GetComponent<Player>().Respawn();
            //balance.Respawn();
        }
    }
}