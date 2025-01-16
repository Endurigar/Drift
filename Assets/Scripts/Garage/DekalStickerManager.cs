using Garage.GarageUI;
using Mods;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class DecalStickerManager : MonoBehaviour
    {
        [SerializeField] private Car car;
        [SerializeField] private Transform decalsHandler;
        [SerializeField] private Button setDecal;
        [SerializeField] private Button deleteDecal;
        [SerializeField] private ChangeWheelsButtonsSpawner changeWheelsButtonsSpawner;
        [SerializeField] private CarModsSo carModsSo;
        [SerializeField] private CarSwapper carSwapper;
        [SerializeField] private GameObject stickersPage;
        public LayerMask carLayerMask;
        public LayerMask decalLayerMask;
        private Decal currentDecal;
        private bool isLoading;

        private void Awake()
        {
            carSwapper.OnCarSwapped += OnCarSwapped;
            setDecal.onClick.AddListener((SetDecal));
            deleteDecal.onClick.AddListener(DeleteDecal);
            changeWheelsButtonsSpawner.OnDecalSelected += DecalChose;
            decalsHandler = car.CurrentCarInstance.DecalHandler.transform;
        }

        void Update()
        {
            OnInput();
        }

        private void OnInput()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, decalLayerMask))
                {
                    currentDecal = hit.collider.gameObject.GetComponent<Decal>();
                }

                ;
            }

            if (Input.GetMouseButton(1) && currentDecal != null)
            {
                MoveDecal();
            }
        }
        private void OnCarSwapped()
        {
            isLoading = true;
            decalsHandler = car.CurrentCarInstance.DecalHandler.transform;
            isLoading = false;
        }

        private void SetDecal()
        {
            currentDecal = null;
            stickersPage.SetActive(false);
        }

        private void DecalChose(string id)
        {
            currentDecal = carModsSo.GetModById(id) as Decal;
            if (car.AddSticker(id, decalsHandler.localPosition, decalsHandler.localEulerAngles))
            {
                currentDecal = Instantiate(currentDecal, decalsHandler.position, decalsHandler.rotation, decalsHandler);
            }
        }

        void MoveDecal()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, carLayerMask))
            {
                currentDecal.transform.position = hit.point;
                AlignDecalToSurface(hit);
                car.MoveSticker(((ICarMod)currentDecal).Id, currentDecal.transform.localPosition,
                    currentDecal.transform.localEulerAngles);
            }
        }

        private void DeleteDecal()
        {
            if (currentDecal != null)
            {
                car.RemoveSticker(((ICarMod)currentDecal).Id);
                Destroy(currentDecal.gameObject);
            }
        }

        void AlignDecalToSurface(RaycastHit hit)
        {
            currentDecal.transform.rotation = Quaternion.LookRotation(-hit.normal);
        }
    }
}