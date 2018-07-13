const domCache = [];
const loaderDom = `<div class="sk-fading-circle">
  <div class="sk-circle1 sk-circle"></div>
  <div class="sk-circle2 sk-circle"></div>
  <div class="sk-circle3 sk-circle"></div>
  <div class="sk-circle4 sk-circle"></div>
  <div class="sk-circle5 sk-circle"></div>
  <div class="sk-circle6 sk-circle"></div>
  <div class="sk-circle7 sk-circle"></div>
  <div class="sk-circle8 sk-circle"></div>
  <div class="sk-circle9 sk-circle"></div>
  <div class="sk-circle10 sk-circle"></div>
  <div class="sk-circle11 sk-circle"></div>
  <div class="sk-circle12 sk-circle"></div>
</div>`;


function generateLoader(id) {
    let elem = null;
    if(id != null) {
        elem = $("#" + id);
    }else{
        elem = $("body");
    }

    domCache.push({id: id,  content: elem.html()});
    elem.html(loaderDom);
}
function removeLoader(id){
    let elem = null;
    if(id != null) {
        elem = $("#" + id);
    }else{
        elem = $("body");
    }
    for (let cache in domCache){
        if (cache.id === id){
            elem.html(cache.content);
        } 
    } 
}