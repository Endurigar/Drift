using System;
using System.IO;
using Fusion;
using Fusion.Sockets;
using Multiplayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mods
{
    public class Car : NetworkBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CarModsSO carModsSO;
        private CarData data;
        private WheelData currentCarInstance;
        public CarData Data => data;

        public WheelData CurrentCarInstance => currentCarInstance;
        private void LoadData()
        {
            var path = Path.Combine(Application.persistentDataPath,$"{CurrentCarInstance.name}.json");
            if (File.Exists(path))
            { 
                var json = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<CarData>(json);
            }
            else
            {
                data = new();
            }
        }

        private CarData GetData(int index)
        {
            var name = carModsSO.CarPrefabs[index].name;
            var path = Path.Combine(Application.persistentDataPath,$"{name}(Clone).json");
            if (File.Exists(path))
            { 
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<CarData>(json);
            }
            else
            {
                return  new();
            }
        }
   
        public void ReceivedData(byte[] dataBytes)
        {
            string data = System.Text.Encoding.UTF8.GetString(dataBytes);
            JObject carData = JObject.Parse(data);
            if (Convert.ToUInt32(carData["PlayerIndex"].ToString()) != Object.InputAuthority.PlayerId) return;
            ChangeCar(Convert.ToInt32(carData["CarIndex"].ToString()),false);
            var modsData = JsonConvert.DeserializeObject<CarData>(carData["CarData"].ToString());
            this.data = modsData;
            LoadModsForCar();
            if (Object.HasInputAuthority)
            {
                cameraTransform.SetParent(currentCarInstance.BodyMesh.transform);
                Camera.main.transform.SetParent(cameraTransform);
                Camera.main.transform.localPosition = Vector3.zero;
                Camera.main.transform.localRotation = Quaternion.identity;
            }
        }
        public void Spawn()
        {
            JObject jsonData = new JObject();
            JToken mods = JToken.FromObject(GetData(PlayerPrefs.GetInt("CurrentCarIndex")));
            jsonData.Add("CarData",mods);
            jsonData.Add("CarIndex",PlayerPrefs.GetInt("CurrentCarIndex"));
            jsonData.Add("PlayerIndex", Object.InputAuthority.PlayerId);
            var players = Runner.ActivePlayers;
            byte[] serializedData = System.Text.Encoding.UTF8.GetBytes(jsonData.ToString());
        
            foreach (var player in players)
            {
                var key = ReliableKey.FromInts(42, 0, 0, 0);
                Runner.SendReliableDataToPlayer(player,key,serializedData); 
            }
        
        }
        public override void Spawned()
        {
            var spawner = FindObjectOfType<BasicSpawner>();
            spawner.OnDataReceived += ReceivedData;
            var transf = Spawner.Instance.SpawnPoints[Object.InputAuthority.PlayerId - 1];
            transform.position = transf.transform.position;
            transform.rotation = transf.transform.rotation;
            base.Spawned();
     
        }
        public void SpawnSavedCar()
        {
       
            Spawn();
        
        }
        public void Save()
        {
            var path = Path.Combine(Application.persistentDataPath,$"{CurrentCarInstance.name}.json");
       
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var json = JsonConvert.SerializeObject(Data,settings);
            File.WriteAllText(path,json);
        }

        public void ChangeCar(int index, bool loadMods = true)
        {
            if(currentCarInstance != null) Destroy(currentCarInstance.gameObject);
            Debug.Log(index);
            currentCarInstance = Instantiate(carModsSO.CarPrefabs[index],transform);
            currentCarInstance.gameObject.layer = LayerMask.NameToLayer("Car");
            LoadData();
            if(loadMods)
                LoadModsForCar();
            PlayerPrefs.SetInt("CurrentCarIndex", index);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        public void ChangeColor(Color color)
        {
            data.Color = new (color);
            Save();
        }
        public void ChangeWheel(string id)
        {
            var wheel = carModsSO.GetModById(id) as Mods.Wheel;
            var oldWheel = carModsSO.GetAllModByTypeFromList<Mods.Wheel>(Data.CarMods);
            if (oldWheel.Length > 0)
            {
                Data.CarMods.Remove(((ICarMod)oldWheel[0]).Id);
            }
            Data.CarMods.Add(((ICarMod)wheel).Id);
            Save();
        }
        public bool AddSticker(string id,Vector3 position, Vector3 euler)
        {
            if (data.CarMods.Contains(id)) return false;
            Data.CarMods.Add(id);
            var decalTransform = new DecalTransformData();
            decalTransform.Position = new SerializableVector3(position);
            decalTransform.Rotation = new SerializableVector3(euler);
            Data.DecalTransformDatas.Add(id,decalTransform);
            Save();
            return true;
        }

        public void RemoveSticker(string id)
        {
            Data.DecalTransformDatas.Remove(id);
            Data.CarMods.Remove(id);
            Save();
        }

        public void MoveSticker(string id, Vector3 position, Vector3 euler)
        {
            var exists = Data.DecalTransformDatas.ContainsKey(id);
            var decalTransform = exists?Data.DecalTransformDatas[id]: new DecalTransformData();
            decalTransform.Position = new SerializableVector3(position);
            decalTransform.Rotation = new SerializableVector3(euler);
            if(!exists)
                Data.DecalTransformDatas.Add(id,decalTransform);
            Save();
        }

        private void LoadModsForCar()
        {
            LoadDecals();
            LoadColor();
            LoadWheels();
        }

        private void LoadDecals()
        {
            var decals = carModsSO.GetAllModByTypeFromList<Decal>(Data.CarMods);
            foreach (var decal in decals)
            {
                var current =Instantiate(decal, currentCarInstance.DecalHandler.transform);
                current.transform.localPosition = data.DecalTransformDatas[((ICarMod)decal).Id].Position.ToVector3();
                current.transform.localEulerAngles = data.DecalTransformDatas[((ICarMod)decal).Id].Rotation.ToVector3();
            }
        }

        private void LoadColor()
        {
            var carMaterial = CurrentCarInstance.BodyMesh.material;
            carMaterial.color = Data.Color.ToUnityColor();
        }

        private void LoadWheels()
        {
            var wheel = carModsSO.GetAllModByTypeFromList<Mods.Wheel>(Data.CarMods);
            if (wheel.Length != 0)
            {
                for (int i = 0; i < CurrentCarInstance.Wheels.Length; i++)
                {
                    var currentWheel = CurrentCarInstance.Wheels[i].WheelMesh.gameObject;
                    var position = currentWheel.transform.position;
                    var rotation = currentWheel.transform.rotation;
                    var scale = currentWheel.transform.localScale;
                    var newWheel = Instantiate(wheel[0], position, rotation, currentWheel.transform.parent);
                    newWheel.transform.localScale = scale;
                    Destroy(currentWheel);
                    CurrentCarInstance.Wheels[i].WheelMesh = newWheel.transform;
                }
            }
        }

    }
}
