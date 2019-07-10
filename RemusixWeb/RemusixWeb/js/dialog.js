// Get the modal comments
// Get the modal
/*function liks(){
  var modalc = document.getElementById('myModalc');
// Get the button that opens the modal
var btnc = document.getElementById("myBtnc");
// Get the <span> element that closes the modal
var spanc = document.getElementById("closec");
// When the user clicks the button, open the modal 
btnc.onclick = function() {
  modalc.style.display = "block";
}
// When the user clicks on <span> (x), close the modal
spanc.onclick = function() {
  modalc.style.display = "none";
}
// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
  if (event.target == modalc) {
    modalc.style.display = "none";
  }
}
}



// Get the modal likes
// Get the modal
var modall = document.getElementById('myModall');
// Get the button that opens the modal
var btnl = document.getElementById("myBtnl");
// Get the <span> element that closes the modal
var spanl = document.getElementById("closel");
// When the user clicks the button, open the modal 
btnl.onclick = function() {
  modall.style.display = "block";
}
// When the user clicks on <span> (x), close the modal
spanl.onclick = function() {
  modall.style.display = "none";
}
// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
  if (event.target == modall) {
    modall.style.display = "none";
  }
}



// Get the modal song info
// Get the modal
var modal = document.getElementById('myModals');
// Get the button that opens the modal
var btn = document.getElementById("myBtns");
// Get the <span> element that closes the modal
var span = document.getElementById("closes");
// When the user clicks the button, open the modal 
btn.onclick = function() {
  modal.style.display = "block";
}
// When the user clicks on <span> (x), close the modal
span.onclick = function() {
  modal.style.display = "none";
}
// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
  if (event.target == modal) {
    modal.style.display = "none";
  }
}*/






var formDialog = document.getElementById('formDialog');
document.getElementById('showformDialog').onclick = function() {  
formDialog.showModal();  
};
document.getElementById('submit').onclick = function() {
formDialog.close(value);
};
document.getElementById('formDialog').addEventListener('close', function() {
alert(formDialog.returnValue);
});





















