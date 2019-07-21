// ==UserScript==
// @name         Brighttalk Download
// @namespace    http://tampermonkey.net/
// @version      0.1
// @description  Add button for downloading videos from brighttalk.
// @author       You
// @include      /https://www\.brighttalk\.com/webcast/*/
// @grant        none
// ==/UserScript==


(function() {
    'use strict';
    console.log("User Script started");
    var btn = document.createElement("BUTTON");
    var t = document.createTextNode("Download Video");
    btn.style = "position: relative;z-index: 2;";
    btn.onclick = function() {launchDownloader()};
    btn.appendChild(t);
    document.body.appendChild(btn);
})();

function launchDownloader()
    {
    console.log("Now we should open our exe with the URLID and the videoID as parameters");
    let iframe = document.getElementsByClassName("embedded-player ng-scope");
    let innerdoc = iframe[0].contentDocument;
    let body = innerdoc.getElementsByClassName("brighttalk");

    let txt2Parse = body[0].children[5].innerHTML;
    let videoURLID = getvideoURLID(txt2Parse);
    let videoID = getvideoID(txt2Parse);
    let title = document.evaluate("/html/body/div[3]/section[1]/h3", document, null, XPathResult.ANY_TYPE, null);
        console.log(title);
        //title = str.replace(" ", "_")
        //Maybe I should clean up spaces so it is friendlier with cmd

    window.open("VideoDownloader:" + videoURLID + "SP" + videoID);
    //window.open("VideoDownloader:" + videoURLID + "SP_123" + videoID + "SP_123" + title); //PK_ CodeLater : I want to save it with the title later
    }

function getvideoURLID(txt2Parse)
    {
      let temp = txt2Parse.split(",")[0];
      return temp.split('"')[3];
    }

function getvideoID(txt2Parse)
    {
     let curlybracket = txt2Parse.split("{")[2];
     let indexofSlash = curlybracket.lastIndexOf("/");
     let temp = curlybracket.substr(indexofSlash);
     let temp1 = temp.split("_")[1];
     return temp1.split(".")[0];
    }







