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

using SharpGDX.Utils;

/** Used on platforms that don't support the extenions, e.g. HTML5 and iOS.
 * @author mzechner */
public class ControllerManagerStub : AbstractControllerManager {

    public override void addListener (ControllerListener listener) {
	}

    public override void removeListener (ControllerListener listener) {
	}

    public override void clearListeners () {
	}
	
	public override Array<ControllerListener> getListeners() {
		return new Array<ControllerListener>();
	}
}
