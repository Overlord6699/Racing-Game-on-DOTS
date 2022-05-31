using UnityEngine;
using UnityEngine.UI;

namespace Drift.Ui
{
    [RequireComponent(typeof(Image))]
    public class ImageFillBinding : PropertyBindingBase<float>
    {
        private Image image;

        [SerializeField] 
        private Remapping remapping = new Remapping(0, 1, 0, 1);

        protected override void OnPropertyValueChanged(float newValue)
        {
            image.fillAmount = remapping.Remap(newValue);
        }

        protected override void OnDataContextChanged(object dataContext)
        {
            if (image == null) image = GetComponent<Image>();
            base.OnDataContextChanged(dataContext);
        }

        protected override IValue<float> GetProperty(object dataContext, string propertyName)
        {
            return dataContext.GetProperty(propertyName).AdaptToFloat();
        }
    }
}