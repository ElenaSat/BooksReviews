export interface Book {
  id: string;
  title: string;
  author: string;
  category: string;
  description: string;
  coverUrl: string;
  averageRating: number;
}

export interface Review {
  id: string;
  bookId: string;
  userId: string;
  userName: string;
  rating: number;
  comment: string;
  createdAt: string | Date;
}

export interface User {
  id: string;
  name: string;
  email: string;
  avatarUrl: string;
  token?: string;
}
