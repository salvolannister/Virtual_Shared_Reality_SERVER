"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("OnAddPrimitive", function (p) {
    var encodedMsg;

    if (p.x === undefined) {
        encodedMsg = " p is undefined"
    } else {
        var encodedMsg ="Name: "+ p.name + " shape: " + p.shape + " position:" + " X " + p.x + " Y " + p.y + " Z " + p.z ;
    }
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});


//$(document).ready(function () {
  
//    $('input[type="checkbox"]').click(function () {
//        if ($(this).is(":checked")) {
//            alert($(this).val());
//        }
//        else if ($(this).is(":not(:checked)")) {
//            alert("Checkbox is unchecked.");
//        }
//    });

//});






connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
    });



document.getElementById("sendButton").addEventListener("click", function (event) {

    var shape = $("input:checked").val();
     
   
   

    var name = document.getElementById("nameInput").value;
   
    var x = document.getElementById("XInput").value;
    var y = document.getElementById("YInput").value;
    var z = document.getElementById("ZInput").value;
   
    var p = { Shape: shape, Name: name, X: x, Y: y, Z: z};
    alert(p.Id);
    connection.invoke("AddPrimitive", p).catch(function (err) {
       
        return console.error(err.toString());
    });
    event.preventDefault();
});

//$("#gravity").on("click", function () {
//    var value = this.checked;
    
//    connection.invoke("GravityChange", value).catch(function(err) {

//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});
// fix the problem with the unchecking 


$("#gravity").change(function () {
        var value = $(this).is(":checked");
        
        connection.invoke("GravityChange", value).catch(function (err) {

            return console.error(err.toString());
        });
        event.preventDefault();
    });

document.getElementById("saveButton").addEventListener("click", function (event) {
    
    connection.invoke("SaveScene").catch(function (err) {

        return console.error(err.toString());
    });
    event.preventDefault();
    $("#msg").animate({
        opacity: 1
    });
  
});


$("#sceneButton").change(function (e) {
    var fileName = e.target.files[0].name;
    alert("filenaname  is " + fileName);
    alert("filename target" + e.target.files[0]);
    connection.invoke("LoadScene", fileName).catch(function (err) {

        return console.error(err.toString());
    });
    event.preventDefault();
});