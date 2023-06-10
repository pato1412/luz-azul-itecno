Imports System.Drawing.Imaging
Imports System.IO
Imports Microsoft.Office.Interop
Imports Newtonsoft.Json
Imports LAFunctions.LuzAzulCommon

Public Class MDIParent1
    Private controlador As Controlador

    Private Sub MDIParent1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Obtengo la instancia del controlador
        controlador = Controlador.GetInstancia()

        If Controlador.GetDebugMode() Then
            Me.Text += "  ------- MODO DEBUG -------"
        End If

        ImgBackground.Image = ChangeOpacity(ImgBackground.Image, 0.6)

        ToolStripStatusLabelEmpresa.Text = "Empresa: " + Controlador.GetRazonSOcial() + " (" + Controlador.GetNombreBaseDB + ")"

        ToolStripStatusLabelUsuario.Text = "Usuario: " + Controlador.GetUsuarioId()

        ToolStripStatusLabelEstablecimiento.Text = "Establecimiento: " + controlador.GetCurrentEstablecimiento.Descripcion

        'Elemento visibles en todas las condiciones (Franquicias - Fabrica - Centro de distribucion)
        StockToolStripMenuItem.Visible = True


        If Controlador.EsFabrica Then
            EnviarPedidoClienteToolStripMenuItem.Visible = True
            ComprasToolStripMenuItem.Visible = False
            AsientosDeCierreDeMesToolStripMenuItem.Visible = False
        Else
            EnviarPedidoClienteToolStripMenuItem.Visible = False
            ComprasToolStripMenuItem.Visible = True
            AsientosDeCierreDeMesToolStripMenuItem.Visible = True
        End If

        'Verifico que elementos del menu corresponden a Fabrica
        If controlador.EsCentroDistribucion() Then
            RecepcionPedidosToolStripMenuItem.Visible = True
            FacturacionToolStripMenuItem.Visible = True
            AprobarPedidosToolStripMenuItem.Visible = True
            PickeoPedidosToolStripMenuItem.Visible = True
            ConfigurarProductosSenasaToolStripMenuItem.Visible = True
            ConfigurarEmpaquesToolStripMenuItem.Visible = True
            ConfigurarFamiliasToleranciasPickeoToolStripMenuItem.Visible = True
        Else
            RecepcionPedidosToolStripMenuItem.Visible = False
            FacturacionToolStripMenuItem.Visible = False
            AprobarPedidosToolStripMenuItem.Visible = False
            PickeoPedidosToolStripMenuItem.Visible = False
            ConfigurarProductosSenasaToolStripMenuItem.Visible = False
            ConfigurarEmpaquesToolStripMenuItem.Visible = False
            ConfigurarFamiliasToleranciasPickeoToolStripMenuItem.Visible = False
        End If

        'Verifico que elementos del menu son de Debug
        If controlador.GetDebugMode() Then
            GenerarArchivoTXTToolStripMenuItem.Visible = True
            GenerarExcelDePruebasToolStripMenuItem.Visible = True
            AdministrarToolStripMenuItem.Visible = True
        Else
            GenerarExcelDePruebasToolStripMenuItem.Visible = False
            GenerarArchivoTXTToolStripMenuItem.Visible = False
            AdministrarToolStripMenuItem.Visible = False
        End If

        HideMenuElement()

    End Sub
    Private Sub HideMenuElement()
        Dim AppPath As String
        'verifico si existe el ejecutable de pedidos
        AppPath = My.Application.Info.DirectoryPath + "\" + LAFunctions.GlobalSetting.NombreAppPedidos
        If (My.Computer.FileSystem.FileExists(AppPath) = False) Then
            EnviarPedidosToolStripMenuItem.Visible = False
        End If

        'verifico si existe el ejecutable de configuracion
        AppPath = My.Application.Info.DirectoryPath + "\" + LAFunctions.GlobalSetting.NombreAppConfiguracion
        If (My.Computer.FileSystem.FileExists(AppPath) = False) Then
            ConfiguracionesToolStripMenuItem.Visible = False
        End If

        'verifico si existe el ejecutable de emision docs
        AppPath = My.Application.Info.DirectoryPath + "\" + LAFunctions.GlobalSetting.NombreAppEmisionDocumentos
        If (My.Computer.FileSystem.FileExists(AppPath) = False) Then
            FacturacionAutomaticaToolStripMenuItem.Visible = False
        End If

        'verifico si existe el ejecutable de recepcion de pedidos
        AppPath = My.Application.Info.DirectoryPath + "\" + LAFunctions.GlobalSetting.NombreAppRecepcionPedidos
        If (My.Computer.FileSystem.FileExists(AppPath) = False) Then
            RecepcionPedidosToolStripMenuItem.Visible = False
        End If


    End Sub
    Public Shared Function ChangeOpacity(ByVal img As Image, ByVal opacityvalue As Single) As Bitmap
        Dim bmp As New Bitmap(img.Width, img.Height)
        Dim graphics__1 As Graphics = Graphics.FromImage(bmp)
        Dim colormatrix As New ColorMatrix With {
            .Matrix33 = opacityvalue
        }
        Dim imgAttribute As New ImageAttributes
        imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.[Default], ColorAdjustType.Bitmap)
        graphics__1.DrawImage(img, New Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height,
         GraphicsUnit.Pixel, imgAttribute)
        graphics__1.Dispose()
        Return bmp
    End Function

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs)
        Dim CalcProcess As Process
        CalcProcess = Process.GetProcessById(Shell("Calc.exe"))

    End Sub

    Private Sub EnviarPedidosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnviarPedidosToolStripMenuItem.Click
        Try
            'Dim ControladorPedidos As luz_azul_pedidos.Controlador
            'ControladorPedidos = luz_azul_pedidos.Controlador.GetInstancia()

            controlador.ClearLog()
            'ControladorPedidos.ClearLog()

            'verifico que no este abierto el aplicativo de pedidos de una franquicia
            For Each frm As Form In Application.OpenForms
                If frm.Name = "frmClientes" Or frm.Name = "frmMain" Then
                    MsgBox("No puedes enviar un pedido de franquicias si tienes abierto el aplicativos de clientes", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, LAFunctions.GlobalSetting.TituloMensajes)
                    Exit Sub
                End If
            Next

            SetGlobalSettingsToPedidos()

            If controlador.GetCurrentEstablecimientoId() <> "" Then

                Controlador.SetLogEvent("Mostrar Formulario MDI de Establecimientos")

                'tengo un solo establecimiento por lo que no muestro el frm de estableciemientos
                'luz_azul_pedidos.Controlador.SetCurrentEstablecimiento(controlador.GetCurrentEstablecimiento())
                'luz_azul_pedidos.Controlador.SetMostrarTodosDepositos(Controlador.GetMostrarTodosDepositos())

                'Dim frmClasif = New luz_azul_pedidos.frmClasificaciones
                'frmClasif.Show()
            Else
                'Ocurrio un error al obtener el o los establecimientos
                Controlador.WriteLogFile("Ocurrio un error al obtener el o los establecimientos")
                MsgBox("No se ha podido obtener el establecimiento seleccionado", MsgBoxStyle.Critical + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
            End If


        Catch ex As Exception
            'Ocurrio un error al obtener el o los establecimientos
            Controlador.WriteLogFile("Error al leer el ejecutable de la aplicacion: " + LAFunctions.GlobalSetting.NombreAppPedidos + " - " + ex.Message)
            MsgBox("Ocurrio un error al leer la aplicacion de pedidos " + vbCrLf + LAFunctions.GlobalSetting.NombreAppPedidos, vbCritical + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
        End Try
    End Sub
    Private Sub SetGlobalSettingsToPedidos()
        'Sincronizo el globalsetings del form MDI con el globalsetting del proyecto pedidos
        'luz_azul_pedidos.Controlador.ClearCliente()
        'luz_azul_pedidos.Controlador.SetEspedidoCliente(False)
        'luz_azul_pedidos.Controlador.SetCuit(Controlador.GetCUIT())
        'luz_azul_pedidos.Controlador.SetRazonSocial(Controlador.GetRazonSOcial())
        'luz_azul_pedidos.Controlador.SetEsFabrica(Controlador.GetEsFabrica())
        'luz_azul_pedidos.Controlador.SetEsAdministrador(Controlador.GetEsAdministrador())
        'luz_azul_pedidos.Controlador.SetUsuarioId(Controlador.GetUsuarioId())
        'luz_azul_pedidos.Controlador.SetDebugMode(Controlador.GetDebugMode())

        'luz_azul_pedidos.GlobalSettings.SMTP = MDI_GlobalSettings.SMTP
        'luz_azul_pedidos.GlobalSettings.MailPort = MDI_GlobalSettings.MailPort
        'luz_azul_pedidos.GlobalSettings.MailFromAddress = MDI_GlobalSettings.MailFromAddress
        'luz_azul_pedidos.GlobalSettings.MailUsername = MDI_GlobalSettings.MailUsername
        'luz_azul_pedidos.GlobalSettings.MailPassword = MDI_GlobalSettings.MailPassword
        'luz_azul_pedidos.GlobalSettings.MailEnableSSL = MDI_GlobalSettings.MailEnableSSL

    End Sub

    Private Sub ImgBackground_Click(sender As Object, e As EventArgs) Handles ImgBackground.Click

    End Sub

    Private Sub RecepcionPedidosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecepcionPedidosToolStripMenuItem.Click
        controlador.ClearLog()

        If controlador.GetCurrentEstablecimientoId() <> "" Then
            SetGlobalSettingsToRecepcionPedidos()

            Controlador.SetLogEvent("GlobalSettings desde el MDI form al proyecto de recepcion de pedidos")

            'Dim frmRecepcion As New Luz_Azul_Recepcion_Pedidos.frmMainRecepcion
            'frmRecepcion.Show()
        Else
            MsgBox("No se ha podido obtener el establecimiento seleccionado", MsgBoxStyle.Critical + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
        End If

    End Sub
    Private Sub SetGlobalSettingsToRecepcionPedidos()
        'Sincronizo el globalsetings del form MDI con el globalsetting del proyecto pedidos
        'Luz_Azul_Recepcion_Pedidos.Controlador.SetDebugMode(Controlador.GetDebugMode())
        'Luz_Azul_Recepcion_Pedidos.Controlador.SetEsFabrica(Controlador.GetEsFabrica())
        'Luz_Azul_Recepcion_Pedidos.Controlador.SetUsuarioId(Controlador.GetUsuarioId())
        'Luz_Azul_Recepcion_Pedidos.Controlador.SetRazonSocial(Controlador.GetRazonSOcial())
        'Luz_Azul_Recepcion_Pedidos.Controlador.SetCuit(Controlador.GetCUIT())
        'Luz_Azul_Recepcion_Pedidos.Controlador.SetCurrentEstablecimiento(controlador.GetCurrentEstablecimiento())
        'Luz_Azul_Recepcion_Pedidos.Controlador.SetEsAdministrador(controlador.GetEsAdministrador())
        'Luz_Azul_Recepcion_Pedidos.Controlador.SetCurrentDeposito(controlador.GetCurrentDepositos())
        'Luz_Azul_Recepcion_Pedidos.Controlador.SetListEstablecimientosDitribucion(controlador.GetListEstablecimientosDitribucion())

        'Luz_Azul_Recepcion_Pedidos.GlobalSettings.SMTP = MDI_GlobalSettings.SMTP
        'Luz_Azul_Recepcion_Pedidos.GlobalSettings.MailPort = MDI_GlobalSettings.MailPort
        'Luz_Azul_Recepcion_Pedidos.GlobalSettings.MailFromAddress = MDI_GlobalSettings.MailFromAddress
        'Luz_Azul_Recepcion_Pedidos.GlobalSettings.MailUsername = MDI_GlobalSettings.MailUsername
        'Luz_Azul_Recepcion_Pedidos.GlobalSettings.MailPassword = MDI_GlobalSettings.MailPassword
        'Luz_Azul_Recepcion_Pedidos.GlobalSettings.MailEnableSSL = MDI_GlobalSettings.MailEnableSSL
    End Sub

    Private Sub ConfigurarFacturacionEstablecimientoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurarFacturacionEstablecimientoToolStripMenuItem.Click
        controlador.ClearLog()

        SetGlobalSettingsToConfigurarFacturacion()
        'Dim frmFacturacion As New Luz_azul_configuraracion.frmConfigFacturacion
        'frmFacturacion.MdiParent = Me
        'frmFacturacion.Show()
    End Sub

    Private Sub SetGlobalSettingsToConfigurarFacturacion()
        'Sincronizo el globalsetings del form MDI con el globalsetting del proyecto pedidos
        'Luz_azul_configuraracion.GlobalSettings.DebugMode = Controlador.GetDebugMode()
        'Luz_azul_configuraracion.GlobalSettings.UsuarioId = Controlador.GetUsuarioId()
        'Luz_azul_configuraracion.GlobalSettings.EsAdministrador = Controlador.GetEsAdministrador()
        'Luz_azul_configuraracion.GlobalSettings.EsFabrica = Controlador.GetEsFabrica()
        'Luz_azul_configuraracion.GlobalSettings.CUIT = Controlador.GetCUIT()
        'Luz_azul_configuraracion.GlobalSettings.RazonSocial = Controlador.GetRazonSOcial()
    End Sub

    Private Sub FacturacionAutomaticaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FacturacionAutomaticaToolStripMenuItem.Click
        controlador.ClearLog()

        'reseteo el valor antes de mostrar el form
        ToolStripStatusLabelEstablecimiento.Text = "Establecimiento: " + controlador.GetCurrentEstablecimiento.Descripcion

        If controlador.GetCurrentEstablecimientoId() <> "" Then

            SetGlobalSettingsToEmisionDoc()

            'Dim frmEmisionDoc = New luz_azul_emision_doc.frmEmisionDoc
            'frmEmisionDoc.Show()
        Else
            MsgBox("No se ha podido obtener el establecimiento seleccionado", MsgBoxStyle.Critical + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
        End If
    End Sub

    Private Sub SetGlobalSettingsToEmisionDoc()
        'Sincronizo el globalsetings del form MDI con el controlador del proyecto emision
        'luz_azul_emision_doc.Controlador.SetCurrentEstablecimiento(controlador.GetCurrentEstablecimiento())
        'luz_azul_emision_doc.Controlador.SetDebugMode(Controlador.GetDebugMode())
        'luz_azul_emision_doc.Controlador.SetEsAdministrador(Controlador.GetEsAdministrador())
        'luz_azul_emision_doc.Controlador.SetUsuarioId(Controlador.GetUsuarioId())
        'luz_azul_emision_doc.Controlador.SetEsFabrica(Controlador.GetEsFabrica())
        'luz_azul_emision_doc.Controlador.SetRazonSOcial(Controlador.GetRazonSOcial())

    End Sub

    Private Sub GenerarExcelDePruebasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GenerarExcelDePruebasToolStripMenuItem.Click
        Dim objApp As Excel.Application
        Dim objBooks As Excel.Workbooks
        Dim objBook As Excel.Workbook
        Dim objSheets As Excel.Sheets
        Dim objSheet As Excel.Worksheet
        Dim nroRowExcel As Integer
        Dim range As Excel.Range

        Dim datestart As Date = Date.Now

        Try

            Me.Cursor = Cursors.WaitCursor

            Dim PathArchivosExcel As String
            Dim dirRoot As String = System.IO.Directory.GetDirectoryRoot(My.Application.Info.DirectoryPath)
            If dirRoot.Contains("\Tempo") Then
                'si el root contiene la palabra tempo es porque es un directorio en red
                PathArchivosExcel = dirRoot + "\Pedidos\tmp\"
            Else
                'Este es un root local 
                PathArchivosExcel = dirRoot + "Tempo\Pedidos\tmp\"
            End If

            Controlador.SetLogEvent("Generar el archivo excel de pruebas ")

            ' Create a new instance of Excel and start a new workbook.
            objApp = New Excel.Application()
            objBooks = objApp.Workbooks
            objBook = objBooks.Add
            objSheets = objBook.Worksheets
            objSheet = objSheets(1)

            nroRowExcel = 1

            Controlador.SetLogEvent("Escribir en la primer celda ")

            objSheet.Cells(nroRowExcel, 1) = "Clasificacion"
            objSheet.Cells(nroRowExcel, 2) = "Cod. Producto"
            objSheet.Cells(nroRowExcel, 3) = "Descripcion"
            objSheet.Cells(nroRowExcel, 4) = "Cant. Pedida"
            objSheet.Cells(nroRowExcel, 5) = "Cant. Sugerida"

            Controlador.SetLogEvent("Seleccionar rango de celdas y darle estilo ")

            range = objSheet.Range("A" + nroRowExcel.ToString(), Reflection.Missing.Value)
            range = range.Resize(1, 5)
            range.Font.Bold = True
            range.Font.Color = Color.White
            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)

            range.Columns.AutoFit()


            'Voy a intetntar generar una carpeta para los archivos excel y verificar si tengo permisos mas que nada por si estoy en una red 
            If (My.Computer.FileSystem.DirectoryExists(PathArchivosExcel) = False) Then
                My.Computer.FileSystem.CreateDirectory(PathArchivosExcel)
            End If


            'guardo la planilla para despues adjuntarla en el mail
            Dim ExcelFilename As String = PathArchivosExcel + "ExcelPruebas  - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"

            'si el archivo existe lo elimino
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                My.Computer.FileSystem.DeleteFile(ExcelFilename)
            End If

            Controlador.SetLogEvent("Grabando el archivo excel de pruebas ")

            objBook.SaveAs(ExcelFilename)
            objBook.Saved = True

            Controlador.SetLogEvent("Cerrando la instancia de excel de pruebas ")

            objBook.Close(SaveChanges:=False)
            objBook = Nothing

            Controlador.SetLogEvent("Cerrando la instancia de excel de pruebas ")

            objApp.Quit()
            releaseObject(objApp)

            End_Excel_App(datestart, Date.Now) ' This closes excel proces

            Me.Cursor = Cursors.Default
            MsgBox("El archivo excel ha sido generado con exito " + vbCrLf + ExcelFilename, MsgBoxStyle.Information, LAFunctions.GlobalSetting.TituloMensajes)
        Catch es As UnauthorizedAccessException
            Me.Cursor = Cursors.Default
            MsgBox("No tienes permiso " + vbCrLf + es.Message, MsgBoxStyle.Critical, LAFunctions.GlobalSetting.TituloMensajes)
            Dim strError As String = "GenerarArchivosExcel Error No tienes permisos - Message: " + es.Message + vbCrLf + JsonConvert.SerializeObject(es)
            Controlador.WriteLogFile(strError)

            End_Excel_App(datestart, Date.Now) ' This closes excel proces
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox("Ocurrio un error al generar el archivo excel " + vbCrLf + ex.Message, MsgBoxStyle.Critical, LAFunctions.GlobalSetting.TituloMensajes)
            'Si el excel no tiene licencia cuando quiere cerrar la app tira un error, por eso debo continuar igual
            Dim strError As String = "GenerarArchivosExcel Error al cerrar al aplicacion - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            Controlador.WriteLogFile(strError)
            'releaseObject(objApp)

            End_Excel_App(datestart, Date.Now) ' This closes excel proces
        End Try

    End Sub
    Private Sub End_Excel_App(datestart As Date, dateEnd As Date)
        Dim xlp() As Process = Process.GetProcessesByName("EXCEL")
        For Each Process As Process In xlp
            If Process.StartTime >= datestart And Process.StartTime <= dateEnd Then
                Process.Kill()
                Exit For
            End If
        Next
    End Sub
    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Sub GenerarArchivoTXTToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GenerarArchivoTXTToolStripMenuItem.Click

        Try

            Me.Cursor = Cursors.WaitCursor
            Dim datestart As Date = Date.Now

            Dim PathArchivosExcel As String
            Dim dirRoot As String = System.IO.Directory.GetDirectoryRoot(My.Application.Info.DirectoryPath)
            If dirRoot.Contains("\Tempo") Then
                'si el root contiene la palabra tempo es porque es un directorio en red
                PathArchivosExcel = dirRoot + "\prueba\"
            Else
                'Este es un root local 
                PathArchivosExcel = dirRoot + "prueba\"
            End If


            'Voy a intetntar generar una carpeta para los archivos excel y verificar si tengo permisos mas que nada por si estoy en una red 
            If (My.Computer.FileSystem.DirectoryExists(PathArchivosExcel) = False) Then
                Controlador.SetLogEvent("Generar el directorio de pruebas: " + PathArchivosExcel)
                My.Computer.FileSystem.CreateDirectory(PathArchivosExcel)
            End If

            Dim myFilePath As String = PathArchivosExcel + "prueba_archivo.txt"

            If (My.Computer.FileSystem.FileExists(myFilePath)) Then
                Controlador.SetLogEvent("Eliminar el archivo txt de pruebas ")
                My.Computer.FileSystem.DeleteFile(myFilePath)
            End If

            Controlador.SetLogEvent("Generar el archivo txt de pruebas ")

            Using sw As New StreamWriter(File.Open(myFilePath, FileMode.Append))
                sw.WriteLine("--------------- Archivo generado automaticamente MDI-FORM " + DateTime.Now + "-----------------------")
                sw.WriteLine("")
            End Using

            Me.Cursor = Cursors.Default
            MsgBox("El archivo TXT ha sido generado con exito" + vbCrLf + myFilePath, MsgBoxStyle.Information, LAFunctions.GlobalSetting.TituloMensajes)
        Catch es As UnauthorizedAccessException
            Me.Cursor = Cursors.Default
            MsgBox("No tienes permiso " + vbCrLf + es.Message, MsgBoxStyle.Critical, LAFunctions.GlobalSetting.TituloMensajes)
            Dim strError As String = "GenerarArchivosExcel Error No tienes permisos - Message: " + es.Message + vbCrLf + JsonConvert.SerializeObject(es)
            Controlador.WriteLogFile(strError)

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox("Ocurrio un error al generar el archivo excel " + vbCrLf + ex.Message, MsgBoxStyle.Critical, LAFunctions.GlobalSetting.TituloMensajes)
            'Si el excel no tiene licencia cuando quiere cerrar la app tira un error, por eso debo continuar igual
            Dim strError As String = "GenerarArchivosExcel Error al cerrar al aplicacion - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            Controlador.WriteLogFile(strError)
        End Try

    End Sub

    Private Sub EnviarPedidoClienteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnviarPedidoClienteToolStripMenuItem.Click
        'Dim ControladorPedidos As luz_azul_pedidos.Controlador

        Try

            'verifico que no este abierto el aplicativo de pedidos de una franquicia
            For Each frm As Form In Application.OpenForms
                If frm.Name = "frmClasificaciones" Or frm.Name = "frmMain" Then
                    MsgBox("No puedes enviar un pedido de cliente si tienes abierto el aplicativos de franquicias", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, LAFunctions.GlobalSetting.TituloMensajes)
                    Exit Sub
                End If
            Next

            'ControladorPedidos = luz_azul_pedidos.Controlador.GetInstancia()

            'controlador.ClearLog()
            'ControladorPedidos.ClearLog()

            SetGlobalSettingsToPedidos()

            If controlador.GetCurrentEstablecimientoId() <> "" Then
                'luz_azul_pedidos.Controlador.SetCurrentEstablecimiento(controlador.GetCurrentEstablecimiento())

                'Dim frmClientes = New luz_azul_pedidos.frmClientes
                'frmClientes.Show()
            Else
                MsgBox("No se ha podido obtener el establecimiento seleccionado", MsgBoxStyle.Critical + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
            End If

        Catch ex As Exception
            'Ocurrio un error al obtener el o los establecimientos
            Controlador.WriteLogFile("Error al leer el ejecutable de la aplicacion: " + LAFunctions.GlobalSetting.NombreAppPedidos + " - " + ex.Message)
            MsgBox("Ocurrio un error al leer la aplicacion de pedidos " + vbCrLf + LAFunctions.GlobalSetting.NombreAppPedidos, vbCritical + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
        End Try

    End Sub

    Private Sub AprobarPedidosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AprobarPedidosToolStripMenuItem.Click

        controlador.ClearLog()

        SetGlobalSettingsToRecepcionPedidos()

        Controlador.SetLogEvent("GlobalSettings desde el MDI form al proyecto de recepcion de pedidos")

        'Dim frmAprobar As New Luz_Azul_Recepcion_Pedidos.frmAprobarPedidos
        'frmAprobar.Show()
    End Sub


    Private Sub RecepcionFacturasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecepcionFacturasToolStripMenuItem.Click
        Try
            controlador.ClearLog()

            If controlador.GetCurrentEstablecimientoId() <> "" Then
                LAFunctions.ControladorCommon.SetCurrentEstablecimiento(controlador.GetCurrentEstablecimiento())

                'Dim frmRecepcion = New luz_azul_compras.frmRecepcionFC
                'frmRecepcion.Show()
            Else
                MsgBox("No se ha podido obtener el establecimiento seleccionado", MsgBoxStyle.Critical + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
            End If
        Catch ex As Exception
            'Ocurrio un error al obtener el o los establecimientos
            Controlador.WriteLogFile("Error al leer el ejecutable de la aplicacion: " + LAFunctions.GlobalSetting.NombreAppCompras + " - " + ex.Message)
            MsgBox("Ocurrio un error al leer la aplicacion de Compras " + vbCrLf + LAFunctions.GlobalSetting.NombreAppCompras, vbCritical + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
        End Try
    End Sub

    Private Sub GenerarOrdenPickeoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GenerarOrdenPickeoToolStripMenuItem.Click
        controlador.ClearLog()

        SetGlobalSettingsToRecepcionPedidos()

        Controlador.SetLogEvent("GlobalSettings desde el MDI form al proyecto de recepcion de pedidos")

        'Dim frmPickeo As New Luz_Azul_Recepcion_Pedidos.frmMainPickeo
        'frmPickeo.Show()

    End Sub

    Private Sub VerOrdenesPickeoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerOrdenesPickeoToolStripMenuItem.Click
        controlador.ClearLog()

        If controlador.GetCurrentEstablecimientoId() <> "" Then
            Controlador.ClearDepositosSeleccionados()
            Dim frmDepos As New frmDepositos()
            frmDepos.ShowDialog()

            If controlador.GetListDepositosSeleccionados.Count > 0 Then

                SetGlobalSettingsToRecepcionPedidos()

                Controlador.SetLogEvent("GlobalSettings desde el MDI form al proyecto de recepcion de pedidos")

                'Dim frmOrdenes As New Luz_Azul_Recepcion_Pedidos.frmListOrdenes
                'frmOrdenes.Show()
            End If
        Else
            MsgBox("No se ha podido obtener el establecimiento seleccionado", MsgBoxStyle.Critical + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
        End If
    End Sub

    Private Sub AdministrarOrdenesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdministrarOrdenesToolStripMenuItem.Click
        controlador.ClearLog()


        If controlador.GetCurrentEstablecimientoId() <> "" Then

            SetGlobalSettingsToRecepcionPedidos()

            Controlador.SetLogEvent("GlobalSettings desde el MDI form al proyecto de recepcion de pedidos")
            'Dim frmOrdenes As New Luz_Azul_Recepcion_Pedidos.frmOrdenes

            'frmOrdenes.Show()
        Else
            MsgBox("No se ha podido obtener el establecimiento seleccionado", MsgBoxStyle.Critical + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
        End If
    End Sub

    Private Sub AgruparOrdenesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AgruparOrdenesToolStripMenuItem.Click
        controlador.ClearLog()

        SetGlobalSettingsToRecepcionPedidos()

        Controlador.SetLogEvent("GlobalSettings desde el MDI form al proyecto de recepcion de pedidos")
        'Dim frmAgrupacion As New Luz_Azul_Recepcion_Pedidos.frmMainAgrupacion

        'frmAgrupacion.Show()

    End Sub

    Private Sub ConfigurarProductosSenasaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurarProductosSenasaToolStripMenuItem.Click
        controlador.ClearLog()

        SetGlobalSettingsToRecepcionPedidos()

        Controlador.SetLogEvent("GlobalSettings desde el MDI form al proyecto de recepcion de pedidos")
        ' Dim frmProductos As New Luz_Azul_Recepcion_Pedidos.frmProductosSenasa

        'frmProductos.Show()

    End Sub

    Private Sub ConfigurarEmpaquesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurarEmpaquesToolStripMenuItem.Click
        controlador.ClearLog()

        SetGlobalSettingsToRecepcionPedidos()

        Controlador.SetLogEvent("GlobalSettings desde el MDI form al proyecto de recepcion de pedidos")
        'Dim frmEmpaques As New Luz_Azul_Recepcion_Pedidos.frmEmpaquesProductos

        'frmEmpaques.Show()

    End Sub

    Private Sub ConfigurarFamiliasToleranciasPickeoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurarFamiliasToleranciasPickeoToolStripMenuItem.Click
        controlador.ClearLog()

        SetGlobalSettingsToRecepcionPedidos()

        Controlador.SetLogEvent("GlobalSettings desde el MDI form al proyecto de recepcion de pedidos")

        'Dim frmFamilias As New Luz_Azul_Recepcion_Pedidos.frmFamiliasProductosPickeo

        'frmFamilias.ShowDialog()

    End Sub




    Private Sub AdministrarPromocionesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdministrarPromocionesToolStripMenuItem.Click
        controlador.ClearLog()

        SetGlobalSettingsToConfigurarFacturacion()

        'Dim frmPromos As New Luz_azul_configuraracion.frmConfigPromos
        'frmPromos.ShowDialog()

    End Sub

    Private Sub ControlDeInventarioToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ControlDeInventarioToolStripMenuItem.Click
        controlador.ClearLog()

        SetGlobalSettingsToConfigurarFacturacion()

        'Dim frmStock As New Luz_azul_configuraracion.frmAjusteStock
        'frmStock.ShowDialog()

    End Sub

    Private Sub CargardeMermasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CargardeMermasToolStripMenuItem.Click
        controlador.ClearLog()

        SetGlobalSettingsToConfigurarFacturacion()
        'Dim frmMerma As New Luz_azul_configuraracion.frmMermas

        'frmMerma.ShowDialog()

    End Sub

    Private Sub AsientosDeCierreDeMesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AsientosDeCierreDeMesToolStripMenuItem.Click
        controlador.ClearLog()

        SetGlobalSettingsToConfigurarFacturacion()

        'Dim frmAsientos As New Luz_azul_configuraracion.frmAsientosCierre

        'frmAsientos.ShowDialog()

    End Sub
End Class
