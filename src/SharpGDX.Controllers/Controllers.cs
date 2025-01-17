/*******************************************************************************
 * Copyright 2011 See AUTHORS file.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 ******************************************************************************/

namespace SharpGDX.Controllers;

using SharpGDX;
using SharpGDX.Utils;
using Utils.Reflect;

/** Provides access to connected {@link Controller} instances. Query the available controllers via {@link #getControllers()}, add
 * and remove global {@link ControllerListener} instances via {@link #addListener(ControllerListener)} and
 * {@link #removeListener(ControllerListener)}. The listeners will be invoked on the rendering thread. The global listeners will
 * be invoked for all events generated by all controllers. Polling a Controller can be done by invoking one of its getter methods.
 * 
 * @author Nathan Sweet */
public class Controllers {
	private static readonly String TAG = "Controllers";
	static readonly ObjectMap<IApplication, ControllerManager> managers = new ObjectMap<IApplication, ControllerManager>();
    /**
     * The class name of a preferred {@link ControllerManager}. If this is null then an appropriate ControllerManager will
     *  automatically be selected. This must be set before any controllers or managers are found.
     */
	public static String preferredManager = null;

	/** Returns an array of connected {@link Controller} instances. This method should only be called on the rendering thread.
	 * 
	 * @return the connected controllers */
	static public Array<Controller> getControllers () {
		initialize();
		return getManager().getControllers();
	}

	/**
	 * @return the controller the player used most recently. This might return null if there is no
	 *         controller connected or the last used connector disconnected
	 */
	public static Controller getCurrent() {
		initialize();
		return getManager().getCurrentController();
	}

	/** Add a global {@link ControllerListener} that can react to events from all {@link Controller} instances. The listener will be
	 * invoked on the rendering thread.
	 * @param listener */
	static public void addListener (ControllerListener listener) {
		initialize();
		getManager().addListener(listener);
	}

	/** Removes a global {@link ControllerListener}. The method must be called on the rendering thread.
	 * @param listener */
	static public void removeListener (ControllerListener listener) {
		initialize();
		getManager().removeListener(listener);
	}
	
	/** Removes every global {@link ControllerListener} previously added. */
	static public void clearListeners () {
		initialize();
		getManager().clearListeners();
	}
	
	/** Returns all listeners currently registered. Modifying this array will result in undefined behaviour. **/
	static public Array<ControllerListener> getListeners() {
		initialize();
		return getManager().getListeners();
	}

	static private ControllerManager getManager () {
		return managers.get(Gdx.app);
	}

	static private void initialize () {
		if (managers.containsKey(Gdx.app)) return;

		String className = null;
		IApplication.ApplicationType type = Gdx.app.getType();
		ControllerManager manager = null;

		if (preferredManager != null) {
			className = preferredManager;
		} else if (type == IApplication.ApplicationType.Android) {
			className = "com.badlogic.gdx.controllers.android.AndroidControllers";
		} else if (type == IApplication.ApplicationType.Desktop) {
			className = "com.badlogic.gdx.controllers.desktop.JamepadControllerManager";
		} else if (type == IApplication.ApplicationType.WebGL) {
			className = "com.badlogic.gdx.controllers.gwt.GwtControllers";
		} else if (type == IApplication.ApplicationType.iOS) {
			className = "com.badlogic.gdx.controllers.IosControllerManager";
		} else {
			Gdx.app.log(TAG, "No controller manager is available for: " + Gdx.app.getType());
			manager = new ControllerManagerStub();
		}

		if (manager == null) {
			try {
				Type controllerManagerClass = ClassReflection.forName(className);
				manager = (ControllerManager)ClassReflection.newInstance< ControllerManager>(controllerManagerClass);
			} catch (Exception ex) {
				throw new GdxRuntimeException("Error creating controller manager: " + className, ex);
			}
		}

		managers.put(Gdx.app, manager);
		IApplication app = Gdx.app;
		Gdx.app.addLifecycleListener(new ControllerLifecycleListener(app));
		Gdx.app.log(TAG, "added manager for application, " + managers.size + " managers active");
	}

    private class ControllerLifecycleListener : ILifecycleListener
    {
        private readonly IApplication _application;

        public ControllerLifecycleListener(IApplication application)
        {
            _application = application;
        }

        public void Resume()
        {
        }

        public void Pause()
        {
        }

        public void Dispose()
        {
            managers.remove(_application);
            Gdx.app.log(TAG, "removed manager for application, " + managers.size + " managers active");

        }
    }
}
