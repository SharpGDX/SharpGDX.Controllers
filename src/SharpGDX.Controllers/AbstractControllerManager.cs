namespace SharpGDX.Controllers;

using SharpGDX.Utils;

public abstract class AbstractControllerManager : ControllerManager {
    protected readonly Array<Controller> controllers = new Array<Controller>();
    private Controller currentController;

    public Array<Controller> getControllers () {
        return controllers;
    }

    public Controller getCurrentController() {
        return currentController;
    }

    public abstract void addListener(ControllerListener listener);
    public abstract void removeListener(ControllerListener listener);
    public abstract Array<ControllerListener> getListeners();
    public abstract void clearListeners();

    /**
     * Manages currentController field. Must be added to controller listeners as first listener
     */
    public class ManageCurrentControllerListener : ControllerAdapter {
        internal readonly AbstractControllerManager _manager;

        public ManageCurrentControllerListener(AbstractControllerManager manager)
        {
            _manager = manager;
        }

        public override void connected(Controller controller) {
            if (_manager.currentController == null) {
                _manager.currentController = controller;
            }
        }

        public override void disconnected(Controller controller) {
            if (_manager.currentController == controller) {
                _manager.currentController = null;
            }
        }

        public override bool buttonDown(Controller controller, int buttonIndex) {
            _manager.currentController = controller;
            return false;
        }

        public override bool buttonUp(Controller controller, int buttonIndex) {
            _manager.currentController = controller;
            return false;
        }

        public override bool axisMoved(Controller controller, int axisIndex, float value) {
            _manager.currentController = controller;
            return false;
        }
    }
}
