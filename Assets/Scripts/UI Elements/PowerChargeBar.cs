using UnityEngine;
using UnityEngine.UI;

namespace UI_Elements
{
    public class PowerChargeBar : MonoBehaviour
    {
        public Image[] powerCharges;
        public int maxNumPowerCharges = 4;
    
        [SerializeField]
        private int currentNumPowerCharges;

        public delegate void PowerChargeBarDelegate();
        public static PowerChargeBarDelegate UpdatePowerChargeBar;

        private void OnEnable()
        {
            // UpdatePowerChargeBar += PowerChargeFiller;
            Door.DoorOpen += ConsumePowerCharge;
            Door.DoorOpen += PowerChargeFiller;
        }

        // Start is called before the first frame update
        void Start()
        {
            currentNumPowerCharges = maxNumPowerCharges;
        }

        // Update is called once per frame
        void Update()
        {
            // FIXME: update on some action (like opening a door)
            // PowerChargeFiller();
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
