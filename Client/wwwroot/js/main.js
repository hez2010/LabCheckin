window.addEventListener('resize', _ => {
  if (DotNet) {
    DotNet.invokeMethodAsync('LabCenter.Client', 'WindowSizeChanged', [innerWidth, innerHeight]);
  }
});

window.utils = {
  getWindowSize: () => [window.innerWidth, window.innerHeight]
};