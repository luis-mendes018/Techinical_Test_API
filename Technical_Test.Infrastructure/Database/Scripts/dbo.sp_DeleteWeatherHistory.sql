USE [WeatherDatabase]
GO

/****** Object:  StoredProcedure [dbo].[sp_DeleteWeatherHistory]    Script Date: 26/08/2025 17:16:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_DeleteWeatherHistory]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM WeatherHistory
    WHERE Id = @Id;

    -- Retorna o número de linhas afetadas (deletadas)
    SELECT @@ROWCOUNT; 
END
GO

