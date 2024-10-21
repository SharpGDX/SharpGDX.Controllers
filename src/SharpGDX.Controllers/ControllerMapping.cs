namespace SharpGDX.Controllers;

/**
 * Default axis and button constants returned by {@link Controller#getMapping}.
 *
 * Note that on some platforms, this may return the general platform mapping. A connected controller
 * might not have all the features, check with {@link Controller#getAxisCount()},
 * {@link Controller#getMaxButtonIndex()}.
 */
public class ControllerMapping {
    public static readonly int UNDEFINED = -1;

    public readonly int axisLeftX;
    public readonly int axisLeftY;
    public readonly int axisRightX;
    public readonly int axisRightY;

    public readonly int buttonA;
    public readonly int buttonB;
    public readonly int buttonX;
    public readonly int buttonY;
    public readonly int buttonBack;
    public readonly int buttonStart;

    public readonly int buttonL1;
    public readonly int buttonL2;
    public readonly int buttonR1;
    public readonly int buttonR2;

    public readonly int buttonDpadUp;
    public readonly int buttonDpadDown;
    public readonly int buttonDpadLeft;
    public readonly int buttonDpadRight;

    public readonly int buttonLeftStick;
    public readonly int buttonRightStick;

    protected ControllerMapping(int axisLeftX, int axisLeftY, int axisRightX, int axisRightY,
                                int buttonA, int buttonB, int buttonX, int buttonY, int buttonBack, int buttonStart,
                                int buttonL1, int buttonL2, int buttonR1, int buttonR2,
                                int buttonLeftStick, int buttonRightStick,
                                int buttonDpadUp, int buttonDpadDown, int buttonDpadLeft, int buttonDpadRight) {
        this.axisLeftX = axisLeftX;
        this.axisLeftY = axisLeftY;
        this.axisRightX = axisRightX;
        this.axisRightY = axisRightY;

        this.buttonA = buttonA;
        this.buttonB = buttonB;
        this.buttonX = buttonX;
        this.buttonY = buttonY;
        this.buttonBack = buttonBack;
        this.buttonStart = buttonStart;
        this.buttonL1 = buttonL1;
        this.buttonL2 = buttonL2;
        this.buttonR1 = buttonR1;
        this.buttonR2 = buttonR2;
        this.buttonLeftStick = buttonLeftStick;
        this.buttonRightStick = buttonRightStick;

        this.buttonDpadUp = buttonDpadUp;
        this.buttonDpadDown = buttonDpadDown;
        this.buttonDpadLeft = buttonDpadLeft;
        this.buttonDpadRight = buttonDpadRight;
    }
}
