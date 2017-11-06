# PRE EXISTING CODE ON GITHUB DIFFERENT BRANCH
There is more code written on a different branch that can be included in the project.
For example a Signature Scanner/Memory helper class, and several UI components.

Definitely take a look.

# OPENGL HOOK
There is supposed to be some function that gets called whenever a device is created, we can use that function to get a handle to the opengl device.
Once we get the handle to the existing device, whe should be able to create our own device and 'SwapBuffers' between them, allowing us to draw an overlay onto the screen.
