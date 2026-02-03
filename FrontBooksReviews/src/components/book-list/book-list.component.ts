import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BookService } from '../../services/book.service';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  template: `
    <div class="space-y-8">
      <!-- Hero / Search Section -->
      <div class="bg-white p-6 rounded-2xl shadow-sm border border-slate-100">
        <h1 class="text-3xl font-bold text-slate-800 mb-6">Descubre tu próximo libro favorito</h1>
        
        <div class="flex flex-col md:flex-row gap-4">
          <div class="flex-grow relative">
            <input 
              type="text" 
              [(ngModel)]="searchQuery" 
              placeholder="Buscar por título, autor..." 
              class="w-full pl-10 pr-4 py-3 rounded-xl border border-slate-200 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 transition-all outline-none"
            >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-slate-400 absolute left-3 top-1/2 transform -translate-y-1/2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
          </div>
          
          <select 
            [(ngModel)]="selectedCategory" 
            class="md:w-48 px-4 py-3 rounded-xl border border-slate-200 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 transition-all outline-none bg-white"
          >
            <option value="">Todas las Categorías</option>
            @for (cat of categories(); track cat) {
              <option [value]="cat">{{ cat }}</option>
            }
          </select>
        </div>
      </div>

      <!-- Book Grid -->
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        @for (book of filteredBooks(); track book.id) {
          <div class="group bg-white rounded-2xl shadow-sm hover:shadow-md border border-slate-100 overflow-hidden transition-all duration-300 flex flex-col h-full">
            <div class="relative aspect-[2/3] overflow-hidden bg-slate-200">
              <img 
                [src]="book.coverUrl" 
                [alt]="book.title" 
                class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
              >
              <div class="absolute top-2 right-2 bg-white/90 backdrop-blur-sm px-2 py-1 rounded-md text-xs font-semibold text-slate-700 shadow-sm">
                {{ book.category }}
              </div>
            </div>
            
            <div class="p-5 flex-grow flex flex-col">
              <h3 class="font-bold text-lg text-slate-800 leading-tight mb-1 line-clamp-2" [title]="book.title">
                {{ book.title }}
              </h3>
              <p class="text-slate-500 text-sm mb-3">{{ book.author }}</p>
              
              <div class="flex items-center mb-4">
                <div class="flex text-yellow-400 text-sm">
                  @for (star of [1,2,3,4,5]; track star) {
                    @if (star <= Math.round(book.averageRating)) {
                      <span>★</span>
                    } @else {
                      <span class="text-slate-200">★</span>
                    }
                  }
                </div>
                <span class="ml-2 text-xs font-medium text-slate-400">{{ book.averageRating | number:'1.1-1' }}</span>
              </div>
              
              <div class="mt-auto">
                <a [routerLink]="['/book', book.id]" class="block w-full text-center py-2.5 bg-slate-50 hover:bg-blue-50 text-slate-700 hover:text-blue-600 font-medium rounded-lg transition-colors border border-slate-100 hover:border-blue-200">
                  Ver Detalles
                </a>
              </div>
            </div>
          </div>
        } @empty {
          <div class="col-span-full py-12 text-center">
            <div class="inline-block p-4 rounded-full bg-slate-100 mb-4">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-slate-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
              </svg>
            </div>
            <h3 class="text-lg font-medium text-slate-800">No se encontraron libros</h3>
            <p class="text-slate-500">Intenta ajustar tu búsqueda o los filtros.</p>
          </div>
        }
      </div>
    </div>
  `
})
export class BookListComponent {
  bookService = inject(BookService);
  
  // Signals for local UI state
  searchQuery = signal('');
  selectedCategory = signal('');
  
  // Get books from service
  books = this.bookService.getBooks();

  Math = Math;

  // Derived state
  categories = computed(() => {
    const cats = new Set(this.books().map(b => b.category));
    return Array.from(cats).sort();
  });

  filteredBooks = computed(() => {
    const q = this.searchQuery().toLowerCase();
    const c = this.selectedCategory();
    
    return this.books().filter(book => {
      const matchesSearch = book.title.toLowerCase().includes(q) || book.author.toLowerCase().includes(q);
      const matchesCategory = c ? book.category === c : true;
      return matchesSearch && matchesCategory;
    });
  });
}