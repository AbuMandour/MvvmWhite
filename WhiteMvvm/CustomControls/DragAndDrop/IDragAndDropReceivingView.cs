namespace WhiteMvvm.CustomControls.DragAndDrop
{
    public interface IDragAndDropReceivingView
    {
        void OnDropReceived(IDragAndDropMovingView view);
    }
}