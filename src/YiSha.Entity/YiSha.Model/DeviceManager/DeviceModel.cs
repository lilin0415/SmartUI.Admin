using System;
using System.Collections.Generic;
using System.Text;
using YiSha.Entity.DeviceManager;
using YiSha.Entity.TestTaskManager;

namespace YiSha.Model.DeviceManager
{
    public class DeviceModel : DeviceEntity
    {
        public bool IsDeployed
        {
            get; set;
        }

        public bool CheckIsDeployed(DeployTaskEntity deployTask)
        {
            if (deployTask == null)
            {
                IsDeployed = false;
                return false;
            }

            if (!string.IsNullOrEmpty(deployTask.AppToken) && AppToken == deployTask.AppToken
                || !string.IsNullOrEmpty(deployTask.DeviceGuid) && Guid == deployTask.DeviceGuid)
            {
                IsDeployed = true;
                return true;
            }
            else
            {
                IsDeployed = false;
                return false;
            }
        }
    }
}
