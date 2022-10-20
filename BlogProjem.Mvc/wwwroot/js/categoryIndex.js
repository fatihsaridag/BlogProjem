$(document).ready(function () {
    /* DataTables start here. */

    $('#categoriesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Ekle',
                attr: {
                    id: "btnAdd",
                },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {
                }
            },
            {
                text: 'Yenile',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'GET',                                                //Kategori listesi almak istediğimiz için GET
                        url: '/Admin/Category/GetAllCategories/',         // Get işlemini hangi urlye(actiona , controllera)
                        contentType: "application/json",                            //Hangi tipte bir iççerik olacak json mu xml mi ? 
                        beforeSend: function () {                                    //beforeSend : Ajax işlemini yapmadan önce yapacağımızı belirttiğimiz yer
                            $('#categoriesTable').hide();                           //Tablomuzun gizlenmesi  
                            $('.spinner-border').show();                            //spinnerın aktif edilmesi
                        },
                        success: function (data) {                                   //Biz ajax get işlemini yaptık ve bize başarılı bir şekilde geldi .Örnek olarak categoryDto
                            const categoryListDto = jQuery.parseJSON(data);         //Gelen veriyi parse ederek aldık ve categoryListDto ya attık.
                            console.log(categoryListDto);
                            if (categoryListDto.ResultStatus === 0) {               // ResultStatus 0 ise categorylistDto başarılı bir şekilde gelmiş demektir.
                                let tableBody = "";
                                $.each(categoryListDto.Categories.$values,          //CategoyListDto nun içindeki kategorilerdeyiz values değeri içeerisimde döneceiz
                                    function (index, category) {                       // Her bir table rowu tablebody içerisine atalım.
                                        //Foreachten gelen kategorileri yazıyoruz.
                                        tableBody += `
                                                <tr>
                                    <td>${category.Id}</td>
                                    <td>${category.Name}</td>
                                    <td>${category.Description}</td>                                            
                                    <td>${convertFirstLetterToUpperCase(category.IsActive.toString())}</td>
                                    <td>${convertFirstLetterToUpperCase(category.IsDeleted.toString())}</td>
                                    <td>${category.Note}</td>
                                    <td>${convertToShortDate(category.CreatedDate)}</td>
                                    <td>${category.CreatedByName}</td>
                                    <td>${convertToShortDate(category.ModifiedDate)}</td>
                                    <td>${category.ModifiedByName}</td>
                                    <td>
                                        <button class="btn btn-primary btn-sm btn-update"  data-id="${category.Id}"><span class="fas fa-edit"></span></button>
                                        <button class="btn btn-danger btn-delete btn-sm" data-id="${category.Id}"> <span class="fas fa-minus-circle"></span></button>
                                    </td>
                                            </tr>`;
                                    });
                                $('#categoriesTable > tbody').replaceWith(tableBody);       //Buradaki değeri tablomuzun içerisindeki tbodyi seçiyip bununla bizim tablebodymizi yer değiştiriyoruz.
                                $('.spinner-border').hide();                                //Spinnerı artık gizliyoruz ve gizlemiş olduğumuz tabloyu geri getiriyoruz.
                                $('#categoriesTable').fadeIn(1400);
                            } else {   //Eğer resultstatus 1 gelirse yani hatalıysa                 
                                $('.spinner-border').hide();                  //Border spinnerı gizle              
                                $('#categoriesTable').fadeIn(100);             // category tablosunu ortaya çıkart
                                toastr.error(`${err.responseText}`, 'Hata!');      //hata mesajı ver.
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            toastr.error(`${err.statusText}`, 'İşlem Başarısız!');
                        }
                    });
                }
            }
        ],
        language: {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
            "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "sInfoEmpty": "Kayıt yok",
            "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
            "sLoadingRecords": "Yükleniyor...",
            "sProcessing": "İşleniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "Eşleşen kayıt bulunamadı",
            "oPaginate": {
                "sFirst": "İlk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Önceki"
            },
            "oAria": {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "0": "",
                    "1": "1 kayıt seçildi"
                }
            }
        }
    });
    /* DataTables end here */
    /* Ajax GET / Getting the _CategoryAddPartial as Modal Form starts from here. */

    $(function () {
        const url = '/Admin/Category/Add/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal('show');
            });
        });
        /* Ajax GET / Getting the _CategoryAddPartial as Modal Form ends here. */
        /* Ajax POST / Posting the FormData as CategoryAddDto starts from here. */

        placeHolderDiv.on('click',
            '#btnSave',
            function (event) {
                event.preventDefault();
                const form = $('#form-category-add');
                const actionUrl = form.attr('action');
                const dataToSend = form.serialize();
                $.post(actionUrl, dataToSend).done(function (data) {
                    console.log(data);
                    const categoryAddAjaxModel = jQuery.parseJSON(data);
                    console.log(categoryAddAjaxModel);
                    const newFormBody = $('.modal-body', categoryAddAjaxModel.CategoryAddPartial);
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    if (isValid) {
                        placeHolderDiv.find('.modal').modal('hide');
                        const newTableRow = `
                                <tr name=" ${categoryUpdateAjaxModel.CategoryDto.Category.Id}">
                                    <td>${categoryAddAjaxModel.CategoryDto.Category.Id}</td>
                                    <td>${categoryAddAjaxModel.CategoryDto.Category.Name}</td>
                                    <td>${categoryAddAjaxModel.CategoryDto.Category.Description}</td>
                                    <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsActive.toString())}</td>  @*  /*Eğer toString() yazmazsak bool formatında göndermiş oluyoruz.*/*@
                                    <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsDeleted.toString())}</td>
                                    <td>${categoryAddAjaxModel.CategoryDto.Category.Note}</td>
                                    <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                                    <td>${categoryAddAjaxModel.CategoryDto.Category.CreatedByName}</td>
                                    <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                                    <td>${categoryAddAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                                    <td>
                                        <button class="btn btn-primary btn-sm btn-update" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-edit"></span></button>
                                        <button class="btn btn-danger btn-delete btn-sm" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"> <span class="fas fa-minus-circle"></span></button>
                                    </td>
                                </tr>`;
                        const newTableRowObject = $(newTableRow);
                        newTableRowObject.hide();
                        $('#categoriesTable').append(newTableRowObject);
                        newTableRowObject.fadeIn(3500);
                        toastr.success(`${categoryAddAjaxModel.CategoryDto.Message}`, 'Başarılı İşlem!');
                    } else { //Eğer isValid Gelmezse toastr mesajı göstericez ve onun içerisinde validasyon mesajımız olacak.
                        let summaryText = "";   
                        $('#validation-summary > ul > li').each(function () {  //Önce validation summaryinin içindeki ul yi ve onun içerisindeki li yi seçiyoruz.  
                            let text = $(this).text();                        //Ve her bir li nin yazısını alıyoruz seçiyoruz Ve bunu text değişkenine atıyoruz.
                            summaryText = `*${text}\n`;                       // Her bir li den aldığımız yazıyı alt alta sıralamak istiyoruyz.
                        });
                        toastr.warning(summaryText);                         //Ve bunu toastr içerisine yazıyoruz. Meslea kategori adını girmedik bize sağ üstte uyarı toastrı verecek.
                    }
                });
            });
    });
    /* Ajax POST / Posting the FormData as CategoryAddDto ends here. */
    /* Ajax POST / Deleting a Category starts from here */



    $(document).on('click',
        '.btn-delete',
        function (event) {                                                   //Eğer kullanıcı sil butonuna bastığında bir swal fire açılıyor olacak.
            event.preventDefault();                                         //Bunu unutmuyoruz.
            const id = $(this).attr('data-id');                             //Hangi buton Üzerine tıklanmışsa buton üzerinden id yi alıyor olacağız. Sil butonumuzda data-id attributesini okumuş oluyoruz ve okumuş oldugumuzu da aşağıda data olarak göndermiş oluyoruz. 
            const tableRow = $(`[name="${id}"]`);
            const categoryName = tableRow.find('td:eq(1)').text();
            Swal.fire({
                title: 'Silmek istediğinize emin misiniz?',
                text: `${categoryName} adlı kategori silinecektir.`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, silmek istiyorum.',
                cancelButtonText: 'Hayır, silmek istemiyorum.'
            }).then((result) => {
                if (result.isConfirmed) {                                       //Eğer kulanıcı evet dediyse
                    $.ajax({
                        type: 'POST',                                           //Nasıl bir işlem yapıyoruz : POST
                        dataType: 'json',                                       //Hangi bir veri tipinde yapıyoruz : Json
                        data: { categoryId: id },                               //action kısmında ne bekliyorsak onu js olarak göndermişş oluyoruz.
                        url: '/Admin/Category/Delete/',               //Hangi url(action)a gidiyor Delete Action, CategoryController
                        success: function (data) {                              //Başarıylıysa öncelikle data parametresini yakalamak olacak
                            const categoryDto = jQuery.parseJSON(data);         //Önce bizlere gelen datayı parse ediyoruz. back end tarafına gönderdik
                            if (categoryDto.ResultStatus === 0) {               // Eğer işlem success ise 
                                Swal.fire(                                      //Swal fire değerini yapıştırıyoruz.
                                    'Silindi!',                                 //Başlığımız
                                    `${categoryDto.Category.Name} adlı kategori başarıyla silinmiştir.`,   //Mesajımız
                                    'success'
                                );
                                tableRow.fadeOut(3500);                          //Ve table rowu ortadan kaldırıyoruz.
                            } else {                                             // Eğer işlem başarısızsa error
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${categoryDto.Message}`,
                                });
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            toastr.err(`${err.responseText}`, "Hata!")
                        }
                    });
                }
            });
        });

    $(function () {
        const url = '/Admin/Category/Update/';
        const  placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click', '.btn-update', function (event) {       /*Öncelikle butonun tıklanma olayını yakalamış olduk.  */
            event.preventDefault();
            const id = $(this).attr('data-id');                         /* Buton üzerindeki attributelerden data-id yi okuyarak id yi  aldık*/
            $.get(url, { categoryId: id }).done(function (data) {       /*Jquery get fonksiyonunu çalıştırarak url parametresi aldık. İkinci parametre olarak da gideceği yerdeki beklenen category-id parametresini id değişkeni ile vermiş olduk. Bu işlem tamamlandığında bizlere partialView geliyor*/
                placeHolderDiv.html(data);                              /*Sen artık placeholderdiv içerisindeki html i  benim sana vereceğim html ile ekrana bas dedik.*/
                placeHolderDiv.find('.modal').modal('show');            /*Onuda modal form classı içerisindeki bir modal olarak göster dedik.*/

            }).fail(function () {
                toastr.error("Bir hata oluştu.");                        /*Eğer bize done işleminden bir hata gelirse fail olarak toastr mesajı gösterdik.*/
            });
        });
        /*Ajax POST / Updating a Category starts from here*/

        placeHolderDiv.on('click', '#btnUpdate', function (event) {                //Kaydet butonuna tıklandığında bir fonksiyon çalışmasını istiyoruz.
            event.preventDefault();                                         //Bu Fonksiyonumuz event preventDefault alıyor
            const form = $('#form-category-update');                        // Göndereceğimiz formu önce seçiyoruz.
            const actionUrl = form.attr('action');                          // Formumuz içerisindeki attributelerden actionu okuyoruz. Ve bu url e formumuzu göndereceğiz
            const dataToSend = form.serialize();                            //Formu göndermek için veriye ihtiyacımız var .  Form içerisinden veriyi okuduk dataToSende attık.
            $.post(actionUrl, dataToSend).done(function (data) {            // Ajax post işlem ile veriyi göndercez. 1.parametre : hangi url ? 2.parametre :  veri  işlem tamamlandığında bize bir fonksiyon oluştursun ve bizlere geriye dönen datayo getirsin
                const categoryUpdateAjaxModel = jQuery.parseJSON(data);     // Öncelikle bizlere gelen bu datayı okuyalım. 
                console.log(categoryUpdateAjaxModel);
                const newFormBody = $('.modal-body', categoryUpdateAjaxModel.CategoryUpdatePartial);  // Yeni form bodysini model içerisindeki partial üzerinden aldık.
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);                          // Yeni formBodyi eskisiyle yer değiştiriyoruz. Eski formBodymiz placeholderdiv içerisinde  
                const isValid = newFormBody.find('[name = "IsValid"]').val() === 'True';              // IsValid attributesine sahip değeri almış oluyoruz ve bu değeri okumuş oluyoruz..
                if (isValid) {                                                                         //Eğer bu değer true ise categoryUpdate işlemi gerçekleşmiş modelle ilgili bir sorun yok dmeekti.
                    placeHolderDiv.find('.modal').modal('hide');                                       // modal sınıfına ait değeri seçtik.Sen bir modalsın ancak gizlenmelisin.
                    //Butona basıldığında tablewrowu yakalamak istemiştik o yüzden name veriyoruz.
                    const newTableRow = `                                                                 
                                <tr name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}">             
                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category.Id}</td> 
                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category.Name}</td>
                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category.Description}</td>
                                    <td>${convertFirstLetterToUpperCase(categoryUpdateAjaxModel.CategoryDto.Category.IsActive.toString())}</td>  @*  /*Eğer toString() yazmazsak bool formatında göndermiş oluyoruz.*/*@
                                    <td>${convertFirstLetterToUpperCase(categoryUpdateAjaxModel.CategoryDto.Category.IsDeleted.toString())}</td>
                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category.Note}</td>
                                    <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category.CreatedByName}</td>
                                    <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                                    <td>
                                        <button class="btn btn-primary btn-sm btn-update" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-edit"></span></button>
                                        <button class="btn btn-danger btn-delete btn-sm" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"> <span class="fas fa-minus-circle"></span></button>
                                    </td>
                                </tr>`;

                    const newTableRowObject = $(newTableRow);                                                        //TableRowu seçtik.
                    const categoryTableRow = $(`[name ="${categoryUpdateAjaxModel.CategoryDto.Category.Id}" ]`);     // TableRowu name attributesinden yakaladık.
                    newTableRowObject.hide();                                                                        // Öncelikle tableRowu şimdilik gizliyoruz.
                    categoryTableRow.replaceWith(newTableRowObject);                                                 //Gizli veri üzerinde bizler yeni tablo verisiyle değiştiriyoruz.
                    newTableRowObject.fadeIn(3500);                                                                  // Değişmiş yeni rowu  3.5 saniyede ekrana tekrar getiriyoruz.
                    toastr.success(`${categoryUpdateAjaxModel.CategoryDto.Message}`, "Başarılı işlem!");             // Bu işlem sonrasında toastr ile mesajımızı veriyoruz.
                } else { //Eğer isValid Gelmezse toastr mesajı göstericez ve onun içerisinde validasyon mesajımız olacak.
                    let summaryText = "";
                    $('#validation-summary > ul > li').each(function () {  //Önce validation summaryinin içindeki ul yi ve onun içerisindeki li yi seçiyoruz.  
                        let text = $(this).text();                        //Ve her bir li nin yazısını alıyoruz seçiyoruz Ve bunu text değişkenine atıyoruz.
                        summaryText = `*${text}\n`;                       // Her bir li den aldığımız yazıyı alt alta sıralamak istiyoruyz.
                    });
                    toastr.warning(summaryText);                         //Ve bunu toastr içerisine yazıyoruz. Meslea kategori adını girmedik bize sağ üstte uyarı toastrı verecek.
                 }
            }).fail(function (response) {                                   
                console.log(response);

            });
        });
    }); 

});