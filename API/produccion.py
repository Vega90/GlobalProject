from finta import TA
import alpaca_trade_api as tradeapi
import numpy as np
import plotly.graph_objects as go
import plotly.io as pio
import pandas as pd
import io

# claves para acceder a la api
api_key_alpaca = 'PKJU9M1H958FUDUBMHBF'
api_secret_alpaca = 'wgmea1KmaT3Xbmg4lNZdaI5Etl7vK19qkBRMYrTd'
endpoint_alcapa = "https://paper-api.alpaca.markets/"

api = tradeapi.REST(api_key_alpaca, api_secret_alpaca, endpoint_alcapa, api_version="v2")

# cargamos los modelos
from joblib import load

modelo_decisionTree = load('modelo_decisionTree.joblib')
modelo_randomForest = load('modelo_randomForest.joblib')
#modelo_svm = load('modelo_svm.joblib')

def obtenerIndicadores(df_datos):

    '''
    Funcion para extraer los indicadores para poder realizar las predicciones

    Parameters
    ----------
    df_datos: pd.DataFrame
        DataFrame que contiene los datos de alpaca
    '''

    # Calcular las Medias Móviles Exponenciales
    df_datos["4EMA"] = TA.EMA(df_datos, 4)
    df_datos["9EMA"] = TA.EMA(df_datos, 9)
    df_datos["18EMA"] = TA.EMA(df_datos, 18)

    # Calcular el MACD utilizando las MME de 12 y 26 días
    macd_12 = TA.MACD(df_datos, 12)
    macd_26 = TA.MACD(df_datos, 26)

    df_datos["12MACD"] = macd_12["MACD"]
    df_datos["12MACD_signal"] = macd_12["SIGNAL"]

    df_datos["26MACD"] = macd_26["MACD"]
    df_datos["26MACD_signal"] = macd_26["SIGNAL"]

    # El índice direccional promedio (ADX)
    df_datos["ADX"] = TA.ADX(df_datos)

    # Calcular el RSI con un período de 14 días
    df_datos["RSI"] = TA.RSI(df_datos, 14)

    # Definir el periodo para el Oscilador Estocástico (normalmente 14 días)
    period = 14

    # Calcular el %K
    df_datos['Lowest Low'] = df_datos['low'].rolling(window=period).min()
    df_datos['Highest High'] = df_datos['high'].rolling(window=period).max()
    df_datos['Stochastic_%K'] = 100 * ((df_datos['close'] - df_datos['Lowest Low']) / (df_datos['Highest High'] - df_datos['Lowest Low']))

    # Calcular el %D (media móvil simple de %K)
    df_datos['Stochastic_%D'] = df_datos['Stochastic_%K'].rolling(window=3).mean()

    # Eliminar las columnas temporales que ya no se necesitan
    df_datos.drop(columns=['Lowest Low', 'Highest High'], inplace=True)

    # Eliminar duplicados
    df_datos = df_datos[~df_datos.index.duplicated(keep='first')]

    # Calcular el On-Balance Volume (OBV)
    df_datos['OBV'] = TA.OBV(df_datos)

    # Calcular la línea de Acumulación/Distribución (A/D)
    df_datos['ADL'] = TA.ADL(df_datos)

    return df_datos

def predecir(ticker, intervalo, fechaInicio, fechaFinal, modelo):
    print("Comenzando a predecir...")
    df_predecir = api.get_bars(ticker, intervalo, fechaInicio, fechaFinal, adjustment='raw').df
    df_predecir = obtenerIndicadores(df_predecir).fillna(0)
    print("Obtenidos los indicadores")
    predicciones = []

    
    if modelo == 'decisionTree':
        predicciones = modelo_decisionTree.predict(df_predecir)
    elif modelo == 'randomForest':
        predicciones = modelo_randomForest.predict(df_predecir)
    #else 
        #predicciones = modelo_svm.predict(df_predecir)

    print("Realizadas las predicciones")
    df_predecir['signal'] = predicciones
    df_predecir['datetime'] = df_predecir.index

    return df_predecir[['datetime','close','high','low','open','volume','signal']]
