using UnityEngine;

namespace Drift.Ui
{
    public class TestBinding : MonoBehaviour
    {
        private TestData testData = new TestData();

        private void Start()
        {
            foreach (var binding in GetComponents<IBinding>())
            {
                binding.DataContext = testData;
            }
        }
    }
}