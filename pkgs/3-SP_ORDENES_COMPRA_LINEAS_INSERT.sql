COMMIT WORK;
SET AUTODDL OFF;
SET TERM ^ ;

/* Stored procedures */

CREATE PROCEDURE "SP_ORDENES_COMPRA_LINEAS_INSERT" 
(
  "ARTICULO_X_PROVEEDOR_ID" INTEGER,
  "CANTIDAD_PEDIDA" NUMERIC(12, 3),
  "PRECIO_UNITARIO" NUMERIC(12, 3),
  "FECHA_RECEPCION" DATE,
  "CANTIDAD_RECIBIDA" NUMERIC(12, 3),
  "PORC_DESCUENTO" NUMERIC(12, 3),
  "ORDEN_COMPRA_CAB_ID" INTEGER,
  "UNIDAD_MEDIDA_COD" VARCHAR(50)
)
RETURNS
(
  "PO_I_ORDEN_COMPRA_LINEA_ID" INTEGER
)
AS
BEGIN EXIT; END ^


ALTER PROCEDURE "SP_ORDENES_COMPRA_LINEAS_INSERT" 
(
  "ARTICULO_X_PROVEEDOR_ID" INTEGER,
  "CANTIDAD_PEDIDA" NUMERIC(12, 3),
  "PRECIO_UNITARIO" NUMERIC(12, 3),
  "FECHA_RECEPCION" DATE,
  "CANTIDAD_RECIBIDA" NUMERIC(12, 3),
  "PORC_DESCUENTO" NUMERIC(12, 3),
  "ORDEN_COMPRA_CAB_ID" INTEGER,
  "UNIDAD_MEDIDA_COD" VARCHAR(50)
)
RETURNS
(
  "PO_I_ORDEN_COMPRA_LINEA_ID" INTEGER
)
AS
BEGIN

    /********************************************/
    /* Tomo el pr�ximo valor del generador.     */
    /********************************************/
    PO_I_ORDEN_COMPRA_LINEA_ID = GEN_ID ( SP_ORDENES_COMPRA_LINEAS_G , 1 );

    /********************************************/
    /* Inserto el nuevo registro en la tabla.   */
    /********************************************/
    INSERT INTO "ORDENES_COMPRA_LINEAS"
    (
	    "ORDEN_COMPRA_LINEA_ID",
		"ARTICULO_X_PROVEEDOR_ID",
		"CANTIDAD_PEDIDA",
		"PRECIO_UNITARIO",
		"FECHA_RECEPCION",
		"CANTIDAD_RECIBIDA",
		"PORC_DESCUENTO",
		"ORDEN_COMPRA_CAB_ID",
		"UNIDAD_MEDIDA_COD"
    )
    VALUES
    (
		:"PO_I_ORDEN_COMPRA_LINEA_ID",
		:"ARTICULO_X_PROVEEDOR_ID",
		:"CANTIDAD_PEDIDA",
		:"PRECIO_UNITARIO",
		:"FECHA_RECEPCION",
		:"CANTIDAD_RECIBIDA",
		:"PORC_DESCUENTO",
		:"ORDEN_COMPRA_CAB_ID",
		:"UNIDAD_MEDIDA_COD"
    );

    SUSPEND;

END
 ^

SET TERM ; ^
COMMIT WORK;
SET AUTODDL ON;
