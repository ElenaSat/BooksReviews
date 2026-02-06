import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Book, Review } from '../models/models';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  // State managed by signals
  private _books = signal<Book[]>([]);
  private _reviews = signal<Review[]>([]);
  private _error = signal<string | null>(null);

  error = this._error.asReadonly();

  constructor() {
    this.loadBooks();
  }

  // Load all books from API
  async loadBooks() {
    try {
      this._error.set(null);
      const books = await lastValueFrom(this.http.get<Book[]>(`${this.apiUrl}/Books`));
      this._books.set(books);
    } catch (error) {
      console.error('Error loading books:', error);
      this._error.set('No se pudieron cargar los libros. Revisa tu conexión.');
    }
  }

  // "CQRS Query": Get all books
  getBooks() {
    return this._books.asReadonly();
  }

  // "CQRS Query": Get book by ID
  getBookById(id: string): Book | undefined {
    return this._books().find(b => b.id === id);
  }

  // "CQRS Query": Get reviews for a book from API
  async loadReviewsForBook(bookId: string) {
    try {
      const reviews = await lastValueFrom(this.http.get<Review[]>(`${this.apiUrl}/Reviews/book/${bookId}`));
      // Add or update reviews in the signal
      this._reviews.update(currentReviews => {
        const otherReviews = currentReviews.filter(r => r.bookId !== bookId);
        return [...otherReviews, ...reviews];
      });
    } catch (error) {
      console.error('Error loading reviews:', error);
    }
  }

  getReviewsForBook(bookId: string) {
    return this._reviews()
      .filter(r => r.bookId === bookId)
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
  }

  // "CQRS Command": Add a review
  async addReview(bookId: string, userId: string, userName: string, rating: number, comment: string) {
    const command = {
      id: 'r-' + Math.floor(Math.random() * 10000).toString(),
      bookId: bookId,
      userId: userId,
      userName: userName,
      rating: rating,
      comment: comment
    };

    try {
      await lastValueFrom(this.http.post(`${this.apiUrl}/Reviews`, command, {
        responseType: 'text'
      }));

      // Refresh reviews and book rating
      await this.loadReviewsForBook(bookId);
      await this.loadBooks();
    } catch (error) {
      console.error('Error adding review:', error);
    }
  }
}