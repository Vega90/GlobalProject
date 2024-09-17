from flask import Flask, jsonify, send_file, request
import yfinance as yf
import numpy as np
import produccion
import io

app = Flask(__name__)


@app.route('/api/tickers/details', methods=['POST'])
def get_ticker_data():
    # Obtener JSON del cuerpo de la solicitud
    data = request.get_json()

    name = data.get('name', 'Apple Inc.')
    ticker = data.get('ticker', 'AAPL')
    periodo = data.get('periodo', '1d')
    intervalo = data.get('intervalo', '5min')
    
    if not name or not ticker:
        return jsonify({'error': 'Ticker es requerido, vuelva a intentarlo'}), 400

    response_data = {}
    stock = yf.Ticker(ticker)
    hist = stock.history(period=periodo, interval=intervalo)

    if not hist.empty:
        # Convertir el DataFrame en una lista de diccionarios
        hist_list = hist.reset_index().to_dict(orient='records')
        
        response_data[name] = hist_list

    return jsonify(response_data)

@app.route('/api/predecir', methods=['POST'])
def predecir_endpoint():
    # Obtener parámetros del cuerpo de la solicitud
    data = request.json
    ticker = data.get('ticker', 'MSFT')
    intervalo = data.get('intervalo', '5Min')
    fechaInicio = data.get('fechaInicio', '2023-01-01')
    fechaFinal = data.get('fechaFinal', '2023-01-25')
    modelo = data.get('modelo', 'decisionTree')

    # Devolver el dataframe como JSON y la imagen PNG del grafico
    df_compras = produccion.predecir(ticker, intervalo, fechaInicio, fechaFinal, modelo)
    response_data = {}
    if not df_compras.empty:
        # Convertir el DataFrame en una lista de diccionarios
        signals_list = df_compras.to_dict(orient='records')
        
        response_data[ticker] = signals_list
    
    print("Enviamos las predicciones")
    # Devolver los datos en formato JSON
    return jsonify(response_data)

if __name__ == '__main__':
    app.run(debug=True)
