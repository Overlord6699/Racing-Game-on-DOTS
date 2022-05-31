namespace Drift.Ui
{
    public class ObjectBinding : PropertyBindingBase<object>
    {
        public override ForwardedBindingStrategy ForwardedBindingStrategy
            => ForwardedBindingStrategy.Accept;
        
        protected override void OnPropertyValueChanged(object newValue)
        {
            this.SetDataContextRecursive(newValue);
        }
        
        protected override IValue<object> GetProperty(object dataContext, string propertyName)
        {
            return dataContext.GetProperty(propertyName).AdaptToObject();
        }
    }
}