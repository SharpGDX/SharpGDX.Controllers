namespace SharpGDX.Controllers;

using SharpGDX.Utils;

/**
 * AbstractController to be used by new Controller implementations. Provides listener notification
 * implementations and default return values
 */
abstract class AbstractController : Disposable, Controller {
	private readonly Array<ControllerListener> listeners = new Array<ControllerListener>();
	private bool connected = true;

	public void dispose() {
		lock (listeners) {
			listeners.clear();
		}
		connected = false;
	}

	protected void notifyListenersButtonUp(int button) {
		Array<ControllerListener> managerListeners = Controllers.getListeners();
        lock (managerListeners) {
			foreach (ControllerListener listener in managerListeners) {
				if (listener.buttonUp(this, button))
					break;
			}
		}

        lock (this.listeners) {
			foreach (ControllerListener listener in this.listeners) {
				if (listener.buttonUp(this, button))
					break;
			}
		}
    }

	protected void notifyListenersButtonDown(int button) {
		Array<ControllerListener> managerListeners = Controllers.getListeners();
        lock (managerListeners) {
			foreach (ControllerListener listener in managerListeners) {
				if (listener.buttonDown(this, button))
					break;
			}
		}

        lock (listeners) {
			foreach (ControllerListener listener in listeners) {
				if (listener.buttonDown(this, button))
					break;
			}
		}
    }

	protected void notifyListenersAxisMoved(int axisNum, float value) {
		Array<ControllerListener> managerListeners = Controllers.getListeners();
        lock (managerListeners) {
			foreach (ControllerListener listener in managerListeners) {
				if (listener.axisMoved(this, axisNum, value))
					break;
			}
		}

        lock (listeners) {
			foreach (ControllerListener listener in listeners) {
				if (listener.axisMoved(this, axisNum, value))
					break;
			}
		}
	}

    public abstract ControllerPowerLevel getPowerLevel();

    public void addListener(ControllerListener controllerListener) {
        lock (listeners) {
			if (!listeners.contains(controllerListener, true))
				listeners.add(controllerListener);
		}
	}

	public void removeListener(ControllerListener controllerListener) {
        lock (listeners) {
			listeners.removeValue(controllerListener, true);
		}
	}

	// methods from advanced interface that are not supported by most controllers

	public bool canVibrate() {
		return false;
	}

	public bool isVibrating() {
		return false;
	}

	public void startVibration(int duration, float strength) {

	}

	public void cancelVibration() {

	}

	public bool supportsPlayerIndex() {
		return false;
	}

    public abstract bool getButton(int buttonCode);
    public abstract float getAxis(int axisCode);
    public abstract string getName();
    public abstract string getUniqueId();
    public abstract int getMinButtonIndex();
    public abstract int getMaxButtonIndex();
    public abstract int getAxisCount();

    public bool isConnected() {
		return connected;
	}

	public int getPlayerIndex() {
		return Controller.PLAYER_IDX_UNSET;
	}

	public void setPlayerIndex(int index) {

	}

    public abstract ControllerMapping getMapping();
}
