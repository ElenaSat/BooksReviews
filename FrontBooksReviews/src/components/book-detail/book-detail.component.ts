import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { BookService } from '../../services/book.service';
import { AuthService } from '../../services/auth.service';
import { AiService } from '../../services/ai.service';
import { Book, Review } from '../../models/models';

@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    @if (book(); as b) {
      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Sidebar: Cover & Quick Stats -->
        <div class="lg:col-span-1 space-y-6">
          <div class="bg-white p-2 rounded-2xl shadow-sm border border-slate-100">
            <img [src]="b.coverUrl" [alt]="b.title" class="w-full rounded-xl object-cover aspect-[2/3]">
          </div>
          
          <div class="bg-white p-6 rounded-2xl shadow-sm border border-slate-100">
            <h3 class="text-sm font-bold uppercase tracking-wider text-slate-400 mb-4">Estadísticas</h3>
            <div class="space-y-4">
              <div class="flex justify-between items-center">
                <span class="text-slate-600">Calificación</span>
                <span class="font-bold text-slate-800 flex items-center gap-1">
                  <span class="text-yellow-400">★</span> {{ b.averageRating | number:'1.1-1' }}
                </span>
              </div>
              <div class="flex justify-between items-center">
                <span class="text-slate-600">Reseñas</span>
                <span class="font-bold text-slate-800">{{ reviews().length }}</span>
              </div>
              <div class="flex justify-between items-center">
                <span class="text-slate-600">Categoría</span>
                <span class="inline-block px-2 py-1 bg-slate-100 rounded text-xs font-semibold text-slate-700">{{ b.category }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Main Content -->
        <div class="lg:col-span-2 space-y-8">
          <!-- Info -->
          <div class="bg-white p-8 rounded-2xl shadow-sm border border-slate-100">
            <h1 class="text-3xl md:text-4xl font-bold text-slate-800 mb-2">{{ b.title }}</h1>
            <p class="text-xl text-slate-500 mb-6">por {{ b.author }}</p>
            
            <div class="prose text-slate-600 mb-6">
              <h3 class="font-bold text-slate-800 mb-2">Sinopsis</h3>
              <p>{{ b.description }}</p>
            </div>

            <!-- AI Insight Feature -->
            <div class="bg-indigo-50 rounded-xl p-5 border border-indigo-100">
               <div class="flex items-start gap-3">
                 <div class="mt-1 flex-shrink-0">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-indigo-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                       <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z" />
                    </svg>
                 </div>
                 <div class="flex-grow">
                    <h4 class="font-bold text-indigo-900 text-sm uppercase tracking-wide mb-1">Análisis IA</h4>
                    @if (aiInsight()) {
                      <p class="text-indigo-800 text-sm leading-relaxed">{{ aiInsight() }}</p>
                    } @else {
                      <div class="flex items-center gap-2">
                        <p class="text-indigo-700 text-sm italic">¿Curioso sobre este libro?</p>
                        <button (click)="generateInsight(b)" class="text-xs bg-indigo-600 hover:bg-indigo-700 text-white px-3 py-1 rounded transition-colors">
                          Generar Análisis
                        </button>
                      </div>
                    }
                 </div>
               </div>
            </div>
          </div>

          <!-- Reviews Section -->
          <div class="space-y-6">
             <div class="flex items-center justify-between">
                <h2 class="text-2xl font-bold text-slate-800">Reseñas</h2>
             </div>

             <!-- Add Review Form -->
             @if (authService.isAuthenticated()) {
               <div class="bg-white p-6 rounded-2xl shadow-sm border border-slate-100">
                 <h3 class="text-lg font-bold text-slate-800 mb-4">Escribir una Reseña</h3>
                 <form [formGroup]="reviewForm" (ngSubmit)="submitReview()">
                    @if (errorMessage()) {
                      <div class="mb-4 p-3 bg-red-50 text-red-600 rounded-lg text-sm font-medium border border-red-100">
                         {{ errorMessage() }}
                      </div>
                    }
                   <div class="mb-4">
                     <label class="block text-sm font-medium text-slate-700 mb-1">Calificación</label>
                     <div class="flex gap-2">
                       @for (star of [1,2,3,4,5]; track star) {
                         <button type="button" 
                           (click)="setRating(star)"
                           class="text-2xl focus:outline-none transition-colors"
                           [class.text-yellow-400]="star <= currentRating()"
                           [class.text-slate-200]="star > currentRating()"
                         >★</button>
                       }
                     </div>
                   </div>
                   <div class="mb-4">
                     <label class="block text-sm font-medium text-slate-700 mb-1">Tu Opinión</label>
                     <textarea formControlName="comment" rows="3" class="w-full px-4 py-3 rounded-xl border border-slate-200 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 outline-none transition-all" placeholder="¿Qué te pareció el libro?"></textarea>
                   </div>
                   <button type="submit" [disabled]="reviewForm.invalid || currentRating() === 0" class="px-6 py-2.5 bg-slate-900 hover:bg-black text-white rounded-lg font-medium transition-colors disabled:opacity-50 disabled:cursor-not-allowed">
                     Publicar Reseña
                   </button>
                 </form>
               </div>
             } @else {
               <div class="bg-blue-50 border border-blue-100 rounded-xl p-6 text-center">
                 <p class="text-blue-800 mb-2">¿Quieres compartir tu opinión?</p>
                 <a routerLink="/login" class="inline-block font-semibold text-blue-600 hover:underline">Inicia sesión para escribir una reseña</a>
               </div>
             }

             <!-- Reviews List -->
             @for (review of reviews(); track review.id) {
               <div class="bg-white p-6 rounded-2xl shadow-sm border border-slate-100">
                 <div class="flex justify-between items-start mb-2">
                   <div class="flex items-center gap-3">
                     <div class="h-10 w-10 rounded-full bg-slate-200 flex items-center justify-center text-slate-500 font-bold text-sm">
                        {{ review.userName.charAt(0) }}
                     </div>
                     <div>
                       <h4 class="font-bold text-slate-800 text-sm">{{ review.userName }}</h4>
                       <span class="text-xs text-slate-400">{{ review.createdAt | date:'mediumDate' }}</span>
                     </div>
                   </div>
                   <div class="flex text-yellow-400 text-sm">
                      @for (s of [1,2,3,4,5]; track s) {
                        <span>{{ s <= review.rating ? '★' : '☆' }}</span>
                      }
                   </div>
                 </div>
                 <p class="text-slate-600 leading-relaxed">{{ review.comment }}</p>
               </div>
             } @empty {
               <div class="text-center py-8 text-slate-500">Aún no hay reseñas. ¡Sé el primero en opinar!</div>
             }
          </div>
        </div>
      </div>
    } @else {
      <div class="flex flex-col items-center justify-center min-h-[50vh]">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mb-4"></div>
        <p class="text-slate-500">Cargando detalles del libro...</p>
      </div>
    }
  `
})
export class BookDetailComponent {
  private route: ActivatedRoute = inject(ActivatedRoute);
  private bookService = inject(BookService);
  authService = inject(AuthService);
  private aiService = inject(AiService);
  private fb = inject(FormBuilder);

  book = signal<Book | undefined>(undefined);
  reviews = signal<Review[]>([]);
  aiInsight = signal<string>('');
  currentRating = signal(0);

  reviewForm = this.fb.group({
    comment: ['', [Validators.required, Validators.minLength(10)]]
  });

  constructor() {
    this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.loadBook(id);
      }
    });
  }

  loadBook(id: string) {
    const foundBook = this.bookService.getBookById(id);
    this.book.set(foundBook);
    if (foundBook) {
      this.bookService.loadReviewsForBook(id).then(() => {
        this.reviews.set(this.bookService.getReviewsForBook(id));
      });
      this.aiInsight.set(''); // Reset insight on new book load
    }
  }

  setRating(rating: number) {
    this.currentRating.set(rating);
  }

  errorMessage = signal('');

  async submitReview() {
    this.errorMessage.set('');
    if (this.reviewForm.valid && this.currentRating() > 0 && this.book()) {
      const user = this.authService.currentUser();
      if (!user) {
        this.errorMessage.set('Debes iniciar sesión para publicar una reseña.');
        return;
      }

      try {
        await this.bookService.addReview(
          this.book()!.id,
          user.id,
          user.name,
          this.currentRating(),
          this.reviewForm.value.comment!
        );

        // Refresh local review list
        this.reviews.set(this.bookService.getReviewsForBook(this.book()!.id));
        this.reviewForm.reset();
        this.currentRating.set(0);

        // Refresh local book to update average rating
        const updatedBook = this.bookService.getBookById(this.book()!.id);
        this.book.set(updatedBook);
      } catch (err) {
        console.error('Review submission failed:', err);
        this.errorMessage.set('Error al publicar la reseña. Inténtalo de nuevo más tarde.');
      }
    }
  }

  async generateInsight(book: Book) {
    this.aiInsight.set('Pensando...');
    const insight = await this.aiService.generateBookInsights(book);
    this.aiInsight.set(insight);
  }
}