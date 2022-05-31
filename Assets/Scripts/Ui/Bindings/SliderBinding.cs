using UnityEngine;
using UnityEngine.UI;

namespace Drift.Ui
{
    [RequireComponent(typeof(Slider))]
    public class SliderBinding : PropertyBindingBase<float>
    {
        [SerializeField] 
        private Remapping remapping = new Remapping(0, 1, 0, 1);

        private Slider slider;

        protected override void OnDataContextChanged(object dataContext)
        {
            if (slider == null)
            {
                slider = GetComponent<Slider>();
                slider.onValueChanged.AddListener(OnSliderValueChanged);
            }
            base.OnDataContextChanged(dataContext);
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float arg0)
        {
            UpdateSourceValue(remapping.RemapInversed(arg0));
        }

        protected override void OnPropertyValueChanged(float newValue)
        {
            slider.value = remapping.Remap(newValue);
        }
    }
}