using System.Collections.Generic;
using UnityEngine.UIElements;

namespace InventorySystem
{
    internal static class VisualElementExtensions
    {
        public static T GetElement<T>(this VisualElement visualElement) where T : VisualElement
        {
            T foundElement = null;
            SearchForElement<T>(visualElement, ref foundElement);

            return foundElement;
        }
        public static List<T> GetElements<T>(this VisualElement visualElement) where T : VisualElement
        {
            List<T> foundElements = new List<T>();
            SearchForElements<T>(visualElement, foundElements);

            return foundElements;
        }

        private static void SearchForElement<T>(VisualElement visualElement, ref T foundElement) where T : VisualElement
        {
            if (visualElement is T)
            {
                foundElement = (T)visualElement;
                return;
            }

            foreach (VisualElement child in visualElement.Children())
                SearchForElement<T>(child, ref foundElement);
        } 
        private static void SearchForElements<T>(VisualElement visualElement, List<T> foundElements) where T : VisualElement
        {
            if (visualElement is T)
                foundElements.Add((T)visualElement);
            
            foreach (VisualElement child in visualElement.Children())
                SearchForElements<T>(child, foundElements);
        }
    }
}
