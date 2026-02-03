import { Injectable } from '@angular/core';
import { GoogleGenAI } from '@google/genai';
import { Book } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class AiService {
  private ai: GoogleGenAI;

  constructor() {
    // @ts-ignore
    const apiKey = (typeof process !== 'undefined' ? process.env['API_KEY'] : '') || 'MOCK_KEY';
    this.ai = new GoogleGenAI({ apiKey });
  }

  async generateBookInsights(book: Book): Promise<string> {
    try {
      const prompt = `Proporciona un resumen breve y atractivo de 3 oraciones y un análisis del libro "${book.title}" de ${book.author}. Enfócate en por qué le gustaría a un lector. Responde en Español.`;

      const response = await this.ai.models.generateContent({
        model: 'gemini-1.5-flash',
        contents: [{ role: 'user', parts: [{ text: prompt }] }]
      });
      return response.text || "Análisis no disponible en este momento.";
    } catch (error) {
      console.error('AI Error:', error);
      return "No se pudo generar el análisis en este momento.";
    }
  }
}