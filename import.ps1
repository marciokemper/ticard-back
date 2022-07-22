[void][system.reflection.Assembly]::LoadFrom("/Users/felipebsouza/dev/bosch/facilitiesapi/src/facilitiesapi/bin/Debug/netcoreapp3.1/MySql.Data.dll")

$sql = new-object MySql.Data.MySqlClient.MySqlConnection;
$server = "contas.controlnex.com.br"
$user = "control"
$password = "Control@Nexx2018"
$database = "control_facilite"
$sql.ConnectionString="server=$server;user id=$user;password=$password;database=$database;pooling=false"

$sqlCmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$sqlCmd.Connection = $sql
$sql.open()

$file = "~/dev/bosch/data.csv"
$fornecedores = Get-Content $file | ConvertFrom-Csv

$idFornecedor = 49
$idUsuario = 199

foreach ($f in $fornecedores) {    
    $idFornecedor = ($idFornecedor + 1)
    $date = (Get-Date -Format "dd-MM-yyyy HH:mm:ss")
    $query = "
    use control_facilite;
    INSERT INTO fornecedor              
        VALUES 
            ({0},
            '{1}',
            '{2}', 
            '{3}',
            '{4}',
            '{5}',
            '',
            '{6}',
            '{7}',
            '{8}',
            '',
            '{9}',
            '{10}',
            1,
            NOW(),
            140,
            NOW(),
            140
            ); " -f $idFornecedor, $f.Codigo, $f.RazaoSocial, $f.CNPJ, $f.CEP, $f.Endereco, $f.Bairro, $f.Cidade, $f.UF, $f.Email, $f.Telefone
    
    # $cmd = Invoke-SQLcmd -ServerInstance 'localhost,3306' -query $query -U 'control' -P 'Con@Nexx2018' -Database 'control_facilities_qa' 
    
    $sqlCmd.CommandText = $query
    $sqlReader = $sqlCmd.ExecuteNonQuery()

    if ($sqlReader[0] -ge 1) {
        $emails = ($f."UsuarioEmail".Trim()).Split(";")
        foreach ($e in $emails) {
            $idUsuario = ($idUsuario + 1)
            $email = ($e.Trim())
            $nome = ($email.Split("@"))[0]

            $query = "use control_facilite;
            INSERT INTO usuario              
            VALUES 
                ({0},
                '{1}',
                '{2}', 
                '{2}',
                '{3}',
                '{4}',
                1,
                0);" -f $idUsuario, $nome, $email, $f.UsuarioSenha, $idFornecedor
            $sqlCmd.CommandText = $query
            $sqlReader = $sqlCmd.ExecuteNonQuery()

        }
    }
}

$sql.close()
