export interface Post {
  id: number;
  userId: string;
  username?: string;
  postId: number;
  authorId: string;
  authorName: string;
  content: string;
  createdAt: string; // ISO date string
  likes?: number;
}
