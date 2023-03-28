using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Elements
{
    public class PowerCharge : MonoBehaviour
    {
        public Image[] powerCharges;
        public int maxNumPowerCharges = 4;
    
        [SerializeField]
        private int currentNumPowerCharges;

        private void OnEnable()
        {
            Door.DoorOpen += ConsumePowerCharge;
            Door.DoorOpen += PowerChargeFiller;
        }

        private void OnDisable()
        {
            Door.DoorOpen -= ConsumePowerCharge;
            Door.DoorOpen -= PowerChargeFiller;
        }

        // Start is called before the first frame update
        void Start()
        {
            currentNumPowerCharges = maxNumPowerCharges;
        }

        private void PowerChargeFiller()
        {
            for (int i = 0; i < powerCharges.Length; i++)
            {
                powerCharges[i].enabled = DisplayPowerCharge(currentNumPowerCharges, i);
            }
        }

        private void ConsumePowerCharge()
        {
            if (currentNumPowerCharges > 0)
            {
                currentNumPowerCharges -= 1;
            }
        }

        private bool DisplayPowerCharge(int currentNumPowerCharges, int powerChargeNum)
        {
            // strictly less than because of indexing, starting at 0 (e.g., 3 charges would fill 0, 1, 2)
            return powerChargeNum < currentNumPowerCharges;
        }
    }
}
