// Copyright (C) 2023 Antonin Rousseau
// 
// aimxr-toolkit-csharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// aimxr-toolkit-csharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with aimxr-toolkit-csharp. If not, see <http://www.gnu.org/licenses/>.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AimXRToolkit.Managers
{

    public class WorkPlaceManager : MonoBehaviour
    {
        [SerializeField]
        public LoaderSource loaderSource = null!;
        private Models.Workplace _workplace = null!;
        private Dictionary<int, ArtifactManager> _artifacts = null!;
        public void Start()
        {
            _artifacts = new Dictionary<int, ArtifactManager>();
        }
        public async Task Spawn()
        {
            foreach (Models.ArtifactInstance instance in _workplace.GetArtifacts())
            {
                var artifact = await SpawnArtifact(instance,
                    AimXRManager.Instance.mode == AimXRManager.MODE.VIRTUAL_REALITY);
                artifact.transform.position = instance.position;
                artifact.transform.rotation = Quaternion.Euler(instance.rotation * Mathf.Rad2Deg);
            }
        }

        public async Task<GameObject> SpawnArtifact(Models.ArtifactInstance instance, bool initLogic = false)
        {
            GameObject artifact = await LoadArtifactModel(instance.artifactId);
            var artifactManager = artifact.AddComponent<ArtifactManager>();
            artifactManager.SetArtifact(await DataManager.GetInstance().GetArtifactAsync(instance.artifactId));
            if (initLogic)
            {
                await artifactManager.InitLogic();
            }
            _artifacts.Add(instance.id, artifactManager);
            return artifact;
        }
        public async Task<GameObject> LoadArtifactModel(int id)
        {
            return await loaderSource.LoadGlb(API.API_URL + API.ROUTE.ARTIFACTS + id + "/model");
        }

        public void SetWorkplace(Models.Workplace workplace)
        {
            _workplace = workplace;
        }
        public Models.Workplace GetWorkplace()
        {
            return _workplace;
        }
        public async Task<List<Models.Activity>> GetCompatibleActivitiesAsync()
        {
            return await DataManager.GetInstance().GetCompatibleActivitiesAsync(_workplace.GetId());
        }
        /// <summary>
        /// Get instanciated artifacts in the workplace by instance id
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, ArtifactManager> GetArtifacts()
        {
            return _artifacts;
        }
    }
}
