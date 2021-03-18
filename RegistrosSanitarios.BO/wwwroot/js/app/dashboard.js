/*
 <div id="container_Cajas">
	<div class="row" id="Cajas">
		<div class="col-md-6 mb-2" id="columna">
			<div class="card text-white bg-secondary o-hidden h-100">
            <div class="card-body">
					<div class="card-body-icon"><i class="fa fa-fw fa-graduation-cap"></i></div>
					<div class="card-title mr-5"><h4>Calendario</h4></div>
					<div class="card-text mr-5">
						<ul>
							<li><a href="/Congresos/Details?id=52" class="small card-link text-white"><b>Evento actualizado en calendario</b> - ICBA Pre-Congreso SAC - Datos modificados: Fecha Fin</a></li>
							<li><a href="/Congresos/Details?id=95" class="small card-link text-white"><b>Evento actualizado en calendario</b> - Congreso de octubre - Datos modificados: Fecha Fin,Países</a></li>
						</ul>
					</div>
                </div>
				<a class="card-footer text-white clearfix small z-1" href="/Home/IndexHistorico?idGrupoNovedad=EVENTOS"><span class="float-left">Historial últimas novedades</span><span class="float-right"><i class="fa fa-angle-right"></i></span></a>
			</div>
		</div>
		<div class="col-md-6 mb-2" id="columna">
			<div class="card text-white bg-success o-hidden h-100">
				<div class="card-body">
					<div class="card-body-icon"><i class="fa fa-fw fa-gift"></i></div>
					<div class="card-title mr-5"><h4>Mis Inscripciones</h4></div>
                <div class="card-text mr-5">
                    <ul>
							<li><a href="/Aprobaciones/Details?idEntidad=1089&amp;esSolicitud=False" class="small card-link text-white"><b>Aprobación inscripción pendiente</b> - Congreso Internacional de Medicina Interna SAM  - Prueba carga como apm Prueba carga como apm</a></li>
							<li><a href="/Aprobaciones/Details?idEntidad=1085&amp;esSolicitud=False" class="small card-link text-white"><b>Aprobación inscripción pendiente</b> - XVIII Congreso Nacional de Medicina Familiar y genera  - Inscripcion prueba Apm</a></li>
                    </ul>
                </div>
            </div>
				<a class="card-footer text-white clearfix small z-1" href="/Home/IndexHistorico?idGrupoNovedad=INSCRIPCIONES"><span class="float-left">Historial últimas novedades</span><span class="float-right"><i class="fa fa-angle-right"></i></span></a>
        </div>
    </div>

	</div>
*/
var colores = {
    "EVENTOS": "bg-secondary",
    "INSCRIPCIONES": "bg-success",
    "SOLICITUDES_CONGRESO": "bg-warning",
    "SOLICITUDES_CURSO": "bg-info"
};

var iconos = {
    "EVENTOS": "fa-graduation-cap",
    "INSCRIPCIONES": "fa-gift",
    "SOLICITUDES_CONGRESO": "fa-wpforms",
    "SOLICITUDES_CURSO": "fa-wpforms"
};

var linksVerMas = {
    "EVENTOS": urls.dashboardHistorico,
    "INSCRIPCIONES": urls.dashboardHistorico,
    "SOLICITUDES_CONGRESO": urls.dashboardHistorico,
    "SOLICITUDES_CURSO": urls.dashboardHistorico
};

var linksDetalle = {
    "EVENTOS": urls.eventoDetalle,
    "INSCRIPCIONES": urls.becaDetalle,
    "SOLICITUDES_CONGRESO": urls.solicitudCongresoDetalle,
    "SOLICITUDES_CURSO": urls.solicitudCursoDetalle
};

var objULTextoCaja = $('<ul></ul>');
var objContainerCajas = $('#container_Cajas');
var objFilaCajas = $('<div class="row" id="Cajas"></div>');

function loadCajas(historico, idGrupoNovedad) {
    $('#loading-spinner-container1').html('<div id="loading-spinner2"><i class="fa fa-spinner fa-spin fa-5x"></i></div>');
    //objFilaCajas.children('div#contenido_datos').remove();

    var grupoActual = '';
    var grupoActualDescripcion = '';
    var objLinkDetalle = '';
    var cantCajas = 0;
    $('#container_Cajas').children('div#Cajas').remove();
    objULTextoCaja = $('<ul></ul>');
    $.getJSON(urls.dashboardCajas, data = { IdGrupoNovedad: idGrupoNovedad }, function (data) {
        
        $.each(data, function (index, obj) {
            if (grupoActual === '') {
                grupoActual = obj.IdGrupoNovedad;
                grupoActualDescripcion = obj.DescripcionGrupoNovedad;
            }
            //console.log(obj);
            console.log("grupoActual " + obj.IdGrupoNovedad + ' ' + idGrupoNovedad );
            
            if (grupoActual !== obj.IdGrupoNovedad) {
                //corte de grupo, tengo que agregar la columna
                procesarGrupo(grupoActual, grupoActualDescripcion, historico);
                grupoActual = obj.IdGrupoNovedad;
                grupoActualDescripcion = obj.DescripcionGrupoNovedad;
                cantCajas++;
                console.log(cantCajas);
                if (cantCajas % 2 === 0) {
                    //alert(objFilaCajas.html());
                    objContainerCajas.append(objFilaCajas);
                    //2 cajas por fila
                    cantCajas = 0;
                    objFilaCajas = $('<div class="row" id="Cajas"></div>');
                }
                objULTextoCaja = $('<ul></ul>');
            }
            if (idGrupoNovedad === obj.IdGrupoNovedad || !historico) {
                if (obj.IdNovedad !== null) {
                    if (obj.IdTipoNovedad === 'APROBACION_INSCRIPCION_PENDIENTE') {
                        objLinkDetalle = $('<li><a href="' + urls.aprobacion + '?idEntidad=' + obj.IdRegistro + '&esSolicitud=False" class="small card-link text-white">' + obj.DescripcionTipoNovedad + '</a></li>');
                    }
                    //else if (obj.IdTipoNovedad === 'APROBACION_SOLICITUD_CONGRESO_PENDIENTE' || obj.IdTipoNovedad === 'APROBACION_SOLICITUD_CURSO_PENDIENTE') {
                    //    objLinkDetalle = $('<li><a href="' + urls.aprobacion + '?idEntidad=' + obj.IdRegistro + '&esSolicitud=True" class="small card-link text-white">' + obj.DescripcionTipoNovedad + '</a></li>');
                    //}
                    else if (obj.IdTipoNovedad === 'BECAS_ASIGNADAS') {
                        objLinkDetalle = $('<li><a href="' + urls.eventoDetalle + '?id=' + obj.IdRegistro + '" class="small card-link text-white">' + obj.DescripcionTipoNovedad + '</a></li>');
                    }
                    else {
                        objLinkDetalle = $('<li><a href="' + linksDetalle[grupoActual] + '?id=' + obj.IdRegistro + '" class="small card-link text-white">' + obj.DescripcionTipoNovedad + '</a></li>');
                    }
                
                        console.log("agrego novedad " + obj.DescripcionTipoNovedad);
                        console.log("grupo " + grupoActual);
                    objULTextoCaja.append(objLinkDetalle);
                }

                
            }
            
        });

        if (idGrupoNovedad === grupoActual || !historico) {
        procesarGrupo(grupoActual, grupoActualDescripcion, historico);

            objContainerCajas.append(objFilaCajas);
        }
    });

    $('#loading-spinner-container1').html('');

}

function procesarGrupo(grupoActual, grupoActualDescripcion, historico) {
    console.log("corte grupo " + grupoActual);
    var objTextoCaja = $('<div class="card-text mr-5"></div>');
    var objContainerColumna = $('<div class="col-md-6 mb-2" id="columna"></div>');
    var objContainerCaja = $('<div class="card text-white ' + colores[grupoActual] + ' o-hidden h-100"></div>'); //varia color segun grupo
    var objCuerpoCaja = $('<div class="card-body"></div>)');
    var objIconoCaja = $('<div class="card-body-icon"><i class="fa fa-fw ' + iconos[grupoActual] + '"></i></div>'); //varia icono segun grupo
    var objTituloCaja = $('<div class="card-title mr-5"><h4>' + grupoActualDescripcion + '</h4></div>'); //varia titulo segun grupo;
    var objFooter = $('<a class="card-footer text-white clearfix small z-1" href="' + urls.dashboardHistorico + '?idGrupoNovedad=' + grupoActual + '"><span class="float-left">Historial últimas novedades</span><span class="float-right"><i class="fa fa-angle-right"></i></span></a>');//varia link segun grupo;
    var objSinNovedades = $('<span class="small text-white">Sin novedades</span>');

    objCuerpoCaja.append(objIconoCaja);
    objCuerpoCaja.append(objTituloCaja);
//alert(objULTextoCaja.html().length == 0);
    if (objULTextoCaja.html().length === 0) {
        objTextoCaja.append(objSinNovedades);
    }
    else {
        objTextoCaja.append(objULTextoCaja);
    }
    //alert(objULTextoCaja.html());
    objCuerpoCaja.append(objTextoCaja);

    objContainerCaja.append(objCuerpoCaja);
    console.log(historico);
    console.log(linksVerMas[grupoActual]);
    if (!historico)
    objContainerCaja.append(objFooter);

    objContainerColumna.append(objContainerCaja);
    objFilaCajas.append(objContainerColumna);

   
}


