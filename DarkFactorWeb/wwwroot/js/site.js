
    //
    // Internal class to post data
    //
    function postData(url, postData)
    {
        // Show some debug
        sData = JSON.stringify(postData);
        console.log("postData:" + url + " => " + sData);

        //debugger;

        $.ajax(url, 
        {
            type: 'POST',
            data: JSON.stringify(postData),
            contentType: "application/json; charset=utf-8",
            //dataType: "json",

            success: function(data){
                console.debug("PostData[OK]:" + data);
                window.location.reload(true);
            },
            failure: function(errMsg) {
                console.debug("PostData[ERROR]:"+ errMsg);
            }
        });
    }
    
    function getSectionValue(sectionId,fieldName) 
    { 
        return $("#" + fieldName + "_" + sectionId).val(); 
    } 

    function getFieldValue(fieldName) 
    { 
        return $("#" + fieldName ).val(); 
    } 
