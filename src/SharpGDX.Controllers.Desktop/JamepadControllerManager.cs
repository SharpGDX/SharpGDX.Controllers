namespace SharpGDX.Controllers.Desktop;

using SharpGDX.Utils;
using SharpGDX.Controllers.Desktop.Support;

public class JamepadControllerManager : AbstractControllerManager , Disposable {
    // assign a Jamepad configuration to this field at game startup to override defaults
    public static Configuration jamepadConfiguration;

    private static bool nativeLibInitialized = false;
    private static ControllerManager controllerManager;

    private readonly CompositeControllerListener compositeListener = new CompositeControllerListener();

    public JamepadControllerManager() {
        compositeListener.addListener(new ManageControllers(this, controllers));

        if (!nativeLibInitialized) {
            if (jamepadConfiguration == null) {
                jamepadConfiguration = new com.studiohartman.jamepad.Configuration();
            }

            controllerManager = new com.studiohartman.jamepad.ControllerManager(jamepadConfiguration);
            controllerManager.initSDLGamepad();

            JamepadControllerMonitor monitor = new JamepadControllerMonitor(controllerManager, compositeListener);
            monitor.run();

            Gdx.app.addLifecycleListener(new JamepadShutdownHook(controllerManager));
            Gdx.app.postRunnable(monitor);

            nativeLibInitialized = true;
        }
    }

    public override void addListener(ControllerListener listener) {
        compositeListener.addListener(listener);
    }

    public override void removeListener(ControllerListener listener) {
        compositeListener.removeListener(listener);
    }

    public override Array<ControllerListener> getListeners() {
        Array<ControllerListener> array = new ();
        array.add(compositeListener);
        return array;
    }

    public override void clearListeners() {
        compositeListener.clear();
        compositeListener.addListener(new ManageControllers(this, controllers));
    }

    public void dispose() {
        controllerManager.quitSDLGamepad();
    }

    /**
     * @see com.studiohartman.jamepad.ControllerManager#addMappingsFromFile(String)
     */
    public static void addMappingsFromFile(String path) //throws IOException, IllegalStateException 
    {
        controllerManager.addMappingsFromFile(path);
    }

    /**
     * Writes last native SDL error to error output.
     * Note: Output might not indicate an error, but could be a warning as well.
     * Use for debugging purposes.
     */
    public static void logLastNativeGamepadError() {
        Gdx.app.error("Jamepad", controllerManager.getLastNativeError());
    }

    // TODO: This is terrible, why is this even a class? -RP
    private class ManageControllers : ManageCurrentControllerListener {
        private readonly Array<Controller> _controllers;

        public ManageControllers(JamepadControllerManager controllerManager, Array<Controller> controllers)
        :base(controllerManager)
        {
            _controllers = controllers;
        }

        public override void connected(Controller controller) {
            lock (_controllers) {
               _controllers.add(controller);
            }
            base.connected(controller);
        }

        public override void disconnected(Controller controller) {
            lock (_controllers) {
                _controllers.removeValue(controller, true);
            }
            base.disconnected(controller);
        }
    }
}
