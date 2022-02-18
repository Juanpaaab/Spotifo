window.ReproductorMaestro = {
  ReproducirJS : function (Cancion_ID) {
    var elem = document.getElementById(Cancion_ID);
    elem.play();
  },

  PausarJS : function (Cancion_ID) {
    var elem = document.getElementById(Cancion_ID);
    elem.pause();
  }
}