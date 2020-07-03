using System.Collections.Generic;

namespace WhiteMvvm.CustomControls.DragAndDrop
{
    public interface IDragAndDropHoverableView
    {
        void OnHovered(List<IDragAndDropMovingView> views);
    }
}