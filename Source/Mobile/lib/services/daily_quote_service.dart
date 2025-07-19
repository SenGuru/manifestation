// lib/daily_quote_service.dart
import 'dart:math';

class DailyQuoteService {
  static final List<Map<String, String>> _quotes = [
    {
      'quote': 'Your thoughts become your reality. Choose them wisely.',
      'author': 'Unknown'
    },
    {
      'quote': 'What you think, you become. What you feel, you attract.',
      'author': 'Buddha'
    },
    {
      'quote': 'The universe conspires to help those who know what they want.',
      'author': 'Paulo Coelho'
    },
    {
      'quote': 'Believe in yourself and all that you are.',
      'author': 'Christian D. Larson'
    },
    {
      'quote': 'You are never too old to set another goal or dream a new dream.',
      'author': 'C.S. Lewis'
    },
    {
      'quote': 'The only impossible journey is the one you never begin.',
      'author': 'Tony Robbins'
    },
    {
      'quote': 'Your limitation—it\'s only your imagination.',
      'author': 'Unknown'
    },
    {
      'quote': 'Great things never come from comfort zones.',
      'author': 'Unknown'
    },
    {
      'quote': 'Dream it. Wish it. Do it.',
      'author': 'Unknown'
    },
    {
      'quote': 'Success doesn\'t just find you. You have to go out and get it.',
      'author': 'Unknown'
    },
    {
      'quote': 'The harder you work for something, the greater you\'ll feel when you achieve it.',
      'author': 'Unknown'
    },
    {
      'quote': 'Dream bigger. Do bigger.',
      'author': 'Unknown'
    },
    {
      'quote': 'Don\'t stop when you\'re tired. Stop when you\'re done.',
      'author': 'Unknown'
    },
    {
      'quote': 'Wake up with determination. Go to bed with satisfaction.',
      'author': 'Unknown'
    },
    {
      'quote': 'Do something today that your future self will thank you for.',
      'author': 'Sean Patrick Flanery'
    },
    {
      'quote': 'Little things make big things happen.',
      'author': 'John Wooden'
    },
    {
      'quote': 'It\'s going to be hard, but hard does not mean impossible.',
      'author': 'Unknown'
    },
    {
      'quote': 'Don\'t wait for opportunity. Create it.',
      'author': 'Unknown'
    },
    {
      'quote': 'Sometimes we\'re tested not to show our weaknesses, but to discover our strengths.',
      'author': 'Unknown'
    },
    {
      'quote': 'The key to success is to focus on goals, not obstacles.',
      'author': 'Unknown'
    },
    {
      'quote': 'Manifest your dreams into reality.',
      'author': 'Unknown'
    },
    {
      'quote': 'Your vibe attracts your tribe.',
      'author': 'Unknown'
    },
    {
      'quote': 'Positive mind, positive vibes, positive life.',
      'author': 'Unknown'
    },
    {
      'quote': 'What you seek is seeking you.',
      'author': 'Rumi'
    },
    {
      'quote': 'Be yourself; everyone else is already taken.',
      'author': 'Oscar Wilde'
    },
  ];

  // Get a random quote for each app launch
  static Map<String, String> getRandomQuote() {
    final random = Random(DateTime.now().millisecondsSinceEpoch);
    return _quotes[random.nextInt(_quotes.length)];
  }

  // Get a consistent daily quote (same quote for the whole day)
  static Map<String, String> getDailyQuote() {
    final dayOfYear = DateTime.now().difference(DateTime(DateTime.now().year, 1, 1)).inDays;
    return _quotes[dayOfYear % _quotes.length];
  }

  // Get a quote based on time of day
  static Map<String, String> getTimeBasedQuote() {
    final hour = DateTime.now().hour;
    final random = Random(DateTime.now().day + hour);
    return _quotes[random.nextInt(_quotes.length)];
  }

  // Get all quotes (for future features)
  static List<Map<String, String>> getAllQuotes() {
    return List.unmodifiable(_quotes);
  }

  // Add a custom quote (for future user-generated content)
  static void addCustomQuote(String quote, String author) {
    _quotes.add({
      'quote': quote,
      'author': author,
    });
  }
}